using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GoogleSpeech
{
    public class GoogleVoice
    {

        private static string GenerateUnique(int length)
        {
            string[] LETTERS = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] DIGITS = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string buffer = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(2);
                if (rnd == 1)
                    buffer += LETTERS[random.Next(LETTERS.Length)];
                else
                    buffer += DIGITS[random.Next(DIGITS.Length)];
            }
            return buffer;
        }
        public static String GoogleSpeechRequest(String flacName, int sampleRate)
        {

            WebRequest request = WebRequest.Create("https://www.google.com/speech-api/full-duplex/v1/up?output=json&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw&pair=" + GenerateUnique(16) + "&lang=tr-TR&pFilter=2&maxAlternatives=10&client=chromium");
            request.Method = "POST";
            byte[] byteArray = File.ReadAllBytes(flacName);
            request.ContentType = "audio/x-flac; rate=" + sampleRate;        
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public static String GoogleSpeechRequest(String wavName, String flacName)
        {
            //return GoogleSpeechRequest(flacName, sampleRate);
            int sampleRate = SoundTools.Wav2Flac(wavName, flacName);
            return GoogleSpeechRequest(wavName, sampleRate);
        }
    }
}
