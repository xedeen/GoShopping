using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShopping.Code
{
  public enum CommandDirection
  {
    Add,
    Remove,
    Send
  }

  public enum CommandType
  {
    Items,
    Limit,
    Notification
  }

  public class LexCmd
  {
    public CommandDirection Direction { get; set; }
    public CommandType CommandType { get; set; }

    public List<string> Items { get; set; }

  }
}
