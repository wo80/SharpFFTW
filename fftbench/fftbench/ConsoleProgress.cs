using System;

namespace fftbench
{
    class ConsoleProgress
    {
        private readonly int _maximumWidth;

        public ConsoleProgress(int maximumWidth)
        {
            _maximumWidth = maximumWidth;
        }

        public void Update(int progress, int total, string text)
        {
            Console.Write(string.Empty.PadRight(_maximumWidth, '\b'));

            string output = string.Format("[{0,2}/{1}] {2}", progress, total, text);

            if (output.Length > _maximumWidth)
            {
                output = output.Substring(0, _maximumWidth);
            }
            else
            {
                output = output.PadRight(_maximumWidth, ' ');
            }

            Console.Write(output);
        }
    }
}
