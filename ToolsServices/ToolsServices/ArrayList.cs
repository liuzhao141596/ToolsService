using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsServices
{
  public  class ArrayList
    {
      public  string BuilderGuidStringCommaSeparated(ArrayList alBuilderData)
      {
          if (alBuilderData == null || alBuilderData.Count == 0) return "";

          string strReturn = "";
          foreach (string str in alBuilderData)
          {
              if (IsGuid(str))
                  strReturn = strReturn + "'" + str + "',";
          }

          return strReturn.TrimEnd(',');
      }

      public  bool IsGuid(string guid)
      {
          try
          {
              if (guid.Length < 36)
              {
                  return false;
              }
              var te = new Guid(guid);
              return true;
          }
          catch
          {
              return false;
          }
      }

    }
}
