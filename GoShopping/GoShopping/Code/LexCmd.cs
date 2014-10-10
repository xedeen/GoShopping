using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShopping.Code
{
  public enum CommandDirection
  {
    Add,
    Remove
  }

  public enum CommandType
  {
    Items,
    Limit,
    Notification,
    SendMessage,
    SendMail
  }

  public class LexCmd
  {
    private LexCmd()
    {
    }

    public CommandDirection Direction { get; set; }
    public CommandType CommandType { get; set; }
    public string ContactName { get; set; }

    public List<string> Items { get; set; }

    public static LexCmd FromString(string s)
    {
      var words = s.Trim().ToLowerInvariant().Split();
      if (words.Any())
      {
        var cmd = words.First();

        try
        {
          string phrase;

          if (cmd.StartsWith("отправить") || cmd.StartsWith("отправь") || cmd.StartsWith("отослать") ||
              cmd.StartsWith("отошли") || cmd.StartsWith("послать") || cmd.StartsWith("пошли") ||
              cmd.StartsWith("выслать") || cmd.StartsWith("вышли"))
          {
            phrase = ExcludeWord(words, 0);
            if (phrase.Contains("почте") || phrase.Contains("почтой") || phrase.Contains("электронке") ||
                phrase.Contains("электронкой") || phrase.Contains("mail") || phrase.Contains("письмо") ||
                phrase.Contains("письмом"))
            {
              phrase = ExcludeWords(phrase.Split(),
                new[] {"почте", "почтой", "письмо", "письмом", "электронке", "электронкой", "mail"});
              phrase = ExcludeWords(phrase.Split(), new[] {"по"}, true);
              return new LexCmd { CommandType = CommandType.SendMail, ContactName = phrase };
            }
            else
            {
              phrase = ExcludeWords(phrase.Split(), new[] {"по", "sms", "смс", "смской", "сообщение", "сообщением"},
                true);
              return new LexCmd { CommandType = CommandType.SendMessage, ContactName = phrase };
            }
          }

          
          if (cmd.StartsWith("удалить") || cmd.StartsWith("убрать")|| cmd.StartsWith("удали") || cmd.StartsWith("убeри"))
          {
            phrase = ExcludeWord(words, 0);
            return new LexCmd
            {
              CommandType = Code.CommandType.Items,
              Direction = CommandDirection.Remove,
              Items = SeparateAsListItems(phrase)
            };
          }

          phrase = ExcludeWords(words, new[] {"купить", "добавить", "купи", "добавь"});
          return new LexCmd
          {
            CommandType = CommandType.Items,
            Direction = CommandDirection.Add,
            Items = SeparateAsListItems(phrase)
          };
        }
        catch
        {
        }
      }
      return null;
    }

    private static List<string> SeparateAsListItems(string source)
    {
      var separators = new[]
      {
        " и ",
        " дальше ",
        " далее ",
        " еще ",
        " запятая ",
        " потом "
      };
      var result = new List<string>();

      var words = source.Trim().ToLowerInvariant().Split(separators, StringSplitOptions.RemoveEmptyEntries);
      foreach (var word in words)
      {
        var name = ExcludeWords(word.Split(), separators.Select(s => s.Trim()).ToArray(), true);
        if (!string.IsNullOrEmpty(name.Trim()))
          result.Add(CapitalizeFirst(name.Trim()));
      }
      return result;
    }

    private static string CapitalizeFirst(string s)
    {
      if (string.IsNullOrEmpty(s))
        return string.Empty;

      return s.First().ToString(CultureInfo.InvariantCulture).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();
    }

    private static string ExcludeWords(string[] words, string[] subStrings, bool exactPhrase = false)
    {
      var result = string.Empty;
      for (int i = 0; i < words.Length; i++)
      {
        var clear = true;
        foreach (var s in subStrings)
        {
          if (exactPhrase ? words[i].Equals(s) : words[i].Contains(s))
          {
            clear = false;
            break;
          }
        }
        if (clear)
          result += string.Format("{0} ", words[i]);
      }
      return result.TrimEnd();
    }

    private static string ExcludeWord(string[] words, int number)
    {
      var result = string.Empty;
      for (int i = 0; i < words.Length; i++)
      {
        if (i != number)
          result += string.Format("{0} ", words[i]);
      }
      return result.TrimEnd();
    }
  }
}
