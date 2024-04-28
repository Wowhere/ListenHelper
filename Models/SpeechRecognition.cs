using System.IO;
using Vosk;

namespace voicio.Models
{
    public class SpeechRecognition
    {
        private readonly VoskRecognizer rec;
        public string Recognize(byte[] buffer)
        {
            using (MemoryStream source = new MemoryStream(buffer))
            {
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    rec.AcceptWaveform(buffer, bytesRead);
                    //if (rec.AcceptWaveform(buffer, bytesRead))
                    //{
                    //    //Console.WriteLine(rec.Result());
                    //    //return rec.Result();
                    //}
                    //else
                    //{
                    //    //Console.WriteLine(rec.PartialResult());
                    //    //return rec.PartialResult();
                    //}
                }
            }
            return rec.FinalResult();
        }
        public SpeechRecognition(string modelpath, float samplerate) {
            Model model = new Model(modelpath);
            rec = new VoskRecognizer(model, samplerate);
            rec.SetMaxAlternatives(0);
            rec.SetWords(true);
        }
    }
}
