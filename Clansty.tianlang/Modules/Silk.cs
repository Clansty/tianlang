using System.Diagnostics;

namespace Clansty.tianlang
{
    public static class Silk
    {
        private const string decoder = "/root/silk-v3-decoder/silk/decoder";
        
        public static string decode(string path)
        {
            Process.Start(decoder, $"{path} {path}.pcm").WaitForExit();
            Process.Start("ffmpeg", 
                $"-f s16le -ar 24000 -ac 1 -i {path}.pcm {path}.mp3").WaitForExit();
            return path + ".mp3";
        }
    }
}