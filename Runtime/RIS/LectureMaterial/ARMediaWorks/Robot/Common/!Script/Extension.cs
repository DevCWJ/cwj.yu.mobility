using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace CWJ.YU.Mobility
{
    public static class Extension
    {
        public static Vector3 UsersidePosConvertToUnityPos(Vector3 usersidePos, float unitDistance)
        {
            float unitySpaceX = usersidePos.y * -unitDistance;
            float unitySpaceY = usersidePos.z * unitDistance;
            float unitySpaceZ = usersidePos.x * unitDistance;

            return new Vector3(unitySpaceX, unitySpaceY, unitySpaceZ);
        }

        public static Vector3 UnityPosConvertToUsersidePos(Vector3 localPos, float unitDistance)
        {
            float usersideY = localPos.x / -unitDistance;
            float usersideZ = localPos.y / unitDistance;
            float usersideX = localPos.z / unitDistance;

            return new Vector3(usersideX, usersideY, usersideZ);
        }

        static StringBuilder _SB;
        public static string UpdateNameViaPos(string objFullName, Vector3 pos, bool isIntVector = true, float multiply = 0.1f)
        {
            if (_SB == null)
                _SB = new StringBuilder();

            string fullNameTmp = objFullName.Trim();
            string displayName;
            string comment = string.Empty;
            if (fullNameTmp.Contains("//"))
            {
                var splits = fullNameTmp.Split("//", 2);
                fullNameTmp = splits[0].TrimEnd();
                comment = "//" + splits[1].Trim();
            }
            if (fullNameTmp.Contains("("))
            {
                var splits = fullNameTmp.Split("(", 2);
                displayName = splits[0].TrimEnd() + " "; //displayName
                //"(" + splits[1];
            }
            else
            {
                displayName = fullNameTmp;
            }

            string x, y;
            pos = pos * multiply;
            if (isIntVector)
            {
                Vector3Int v = Vector3Int.FloorToInt(pos);
                x = v.x.ToString();
                y = v.y.ToString();
            }
            else
            {
                x = pos.x.ToString("N1");
                y = pos.y.ToString("N1");
            }

            _SB.Append(displayName);
            _SB.Append("(");
            _SB.Append(x);
            _SB.Append(",");
            _SB.Append(y);
            _SB.Append(")");
            _SB.Append(comment);
            string name = _SB.ToString();
            _SB.Clear();
            return name;
        }

    }
}
