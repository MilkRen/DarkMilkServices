namespace ServerTCP
{
    public static class ConsoleExtension
    {
        public static void WriteLineColor(string text, ConsoleColor foregroundColor = System.ConsoleColor.Gray, ConsoleColor backgroundColor = System.ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteColor(string text, ConsoleColor foregroundColor = System.ConsoleColor.Gray, ConsoleColor backgroundColor = System.ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
