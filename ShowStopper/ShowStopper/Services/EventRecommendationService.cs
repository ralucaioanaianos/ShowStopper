using Firebase.Database;
using ShowStopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Services
{
    class EventRecommendationService
    {
        private static string databaseUrl = "https://showstopper-71398-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task<List<AppEvent>> GetSimilarEvents(AppEvent currentEvent, double relevanceThreshold)
        {
            try
            {
                var firebaseClient = new FirebaseClient(databaseUrl);
                var events = new List<AppEvent>();
                var eventsQuery = firebaseClient.Child("Events").OnceAsync<AppEvent>();
                var eventsSnapshot = await eventsQuery;
                foreach (var snapshot in eventsSnapshot)
                {
                    events.Add(snapshot.Object);
                }
                var similarEvents = CalculateSimilarity(currentEvent, events);
                var relevantEvents = similarEvents.Where(e => e.Relevance >= relevanceThreshold).ToList();
                relevantEvents.Sort((a, b) => b.Relevance.CompareTo(a.Relevance));
                return relevantEvents;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Get Similar Events", ex.Message, "OK");
                return null;
            }
        }

        private static List<AppEvent> CalculateSimilarity(AppEvent currentEvent, List<AppEvent> allEvents)
        {
            var similarEvents = new List<AppEvent>();
            foreach (var eventItem in allEvents)
            {
                if (eventItem != currentEvent)
                {
                    double similarityScore = CalculateSimilarityScore(currentEvent, eventItem);
                    eventItem.Relevance = similarityScore;
                    similarEvents.Add(eventItem);
                }
            }
            return similarEvents;
        }

        private static double CalculateSimilarityScore(AppEvent event1, AppEvent event2)
        {
            double similarityScore = 0.0;
            if (event1.Type == event2.Type)
                similarityScore += 0.3;
            if (event1.Location == event2.Location)
                similarityScore += 0.2;
            similarityScore += CalculateDescriptionSimilarityIncrement(event1.Description, event2.Description);
            return similarityScore;
        }

        public static double CalculateDescriptionSimilarityIncrement(string description1, string description2)
        {
            List<string> words1 = PreprocessAndTokenize(description1);
            List<string> words2 = PreprocessAndTokenize(description2);
            Dictionary<string, double> tfidf1 = CalculateTFIDF(words1);
            Dictionary<string, double> tfidf2 = CalculateTFIDF(words2);
            double cosineSimilarity = CalculateCosineSimilarity(tfidf1, tfidf2);
            double similarityIncrement = cosineSimilarity * 0.3; // Adjust as needed
            return similarityIncrement;
        }

        private static List<string> PreprocessAndTokenize(string text)
        {
            string cleanedText = new string(text.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
            List<string> words = cleanedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return words;
        }

        private static Dictionary<string, double> CalculateTFIDF(List<string> words)
        {
            Dictionary<string, double> tfidfVector = new Dictionary<string, double>();
            Dictionary<string, int> termFrequency = new Dictionary<string, int>();
            foreach (string word in words)
            {
                if (termFrequency.ContainsKey(word))
                {
                    termFrequency[word]++;
                }
                else
                {
                    termFrequency[word] = 1;
                }
            }
            int numDocuments = 2; 
            Dictionary<string, double> inverseDocumentFrequency = new Dictionary<string, double>();
            foreach (string word in words.Distinct())
            {
                int documentsContainingWord = words.Count(w => w == word);
                double idf = Math.Log((double)numDocuments / (double)(documentsContainingWord + 1)); 
                inverseDocumentFrequency[word] = idf;
            }
            foreach (var kvp in termFrequency)
            {
                double tfidf = kvp.Value * inverseDocumentFrequency[kvp.Key];
                tfidfVector[kvp.Key] = tfidf;
            }
            return tfidfVector;
        }

        private static double CalculateCosineSimilarity(Dictionary<string, double> vector1, Dictionary<string, double> vector2)
        {
            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;
            foreach (var kvp in vector1)
            {
                if (vector2.ContainsKey(kvp.Key))
                {
                    dotProduct += kvp.Value * vector2[kvp.Key];
                }
                magnitude1 += Math.Pow(kvp.Value, 2);
            }
            foreach (var kvp in vector2)
            {
                magnitude2 += Math.Pow(kvp.Value, 2);
            }
            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0;
            }
            double cosineSimilarity = dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
            return cosineSimilarity;
        }
    }
}
