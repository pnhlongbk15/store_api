namespace Store.Utils
{
    public static class ErrorSignUp
    {

        public static dynamic Messages { get; set; }

        public static dynamic GetMessage()
        {
            List<string> messages = new List<string>();
            foreach (var m in Messages)
            {
                messages.Add(m.Description.ToString());
            }
            return messages;
        }
    }
}
