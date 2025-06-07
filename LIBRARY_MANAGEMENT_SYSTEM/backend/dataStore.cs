using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBRARY_MANAGEMENT_SYSTEM.backend
{
    public static class dataStore
    {
        public static string? CurrentUsername { get; set; }
    }

    public class UserNotification
    {
        public string Title { get; set; }
        public DateTime DateAdded { get; set; }
        public string Action { get; set; }

        public string DateText => DateAdded.ToString("MM - dd - yyyy");

        public string Message =>
            Action.ToLower() switch
            {
                "borrow" => $"You have requested to borrow a book titled \"{Title}\".",
                "return" => $"You have requested to return a book titled \"{Title}\".",
                _ => $"You have an action on the book titled \"{Title}\"."
            };
    }

}
