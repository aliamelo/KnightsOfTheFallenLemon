using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;

namespace JSONSTUFF
{
    class JSONElement
    {
        public enum JSONType
        {
            DIC,
            LIST,
            STR,
            NB,
            BOOL,
            NULL
        }

        private JSONType type;

        public bool bool_value;
        public int int_value;
        public string string_value;
        public List<JSONElement> data;
        public List<string> key;

        public JSONElement(JSONType type)
        {
            this.type = type;
            if (type == JSONType.LIST || type == JSONType.DIC)
                this.data = new List<JSONElement>();
            if (type == JSONType.DIC)
                this.key = new List<string>();
        }

        public JSONElement(string content)
        {
            this.type = JSONType.STR;
            string_value = content;
        }

        public JSONElement(int content)
        {
            this.type = JSONType.NB;
            int_value = content;
        }

        public JSONType Type
        {
            get { return this.type; }
        }

        public void Add(string key, JSONElement data)
        {
            this.key.Add(key);
            this.data.Add(data);
        }

    }

    static class JSON
    {

        public static JSONElement.JSONType GetJsonType(char c)
        {
            if (c >= '0' && c <= '9' || c == '-')
                return JSONElement.JSONType.NB;
            if (c == '[' || c == ']')
                return JSONElement.JSONType.LIST;
            if (c == '{')
                return JSONElement.JSONType.DIC;
            else return JSONElement.JSONType.STR;
        }

        public static string ParseString(string json, ref int index)
        {
            if (json[index + 1] != ',' && json[index + 1] != ':' && json[index + 1] != '{' && json[index + 1] != '}')
                return json[index++] + ParseString(json, ref index);
            return "" + json[index];
        }


        public static int ParseInt(string json, ref int index)
        {
            char c = json[index];
            int sum = 0;
            while (c >= '0' && c <= '9')
            {
                sum = 10 * sum + json[index++] - '0';
                c = json[index];
            }
            return sum;
        }

        public static bool ParseBool(string json, ref int index)
        {
            bool sortie = json[index] == 't';
            index += sortie ? 4 : 5;
            return sortie;
        }

        public static void EatBlank(string json, ref int index)
        {
            char c = json[index];
            while (c == ' ' || c == '\t' || c == '\n')
            {               
                index++;
                c = json[index];
            }
        }

        public static JSONElement ParseJSONString(string json, ref int index)
        {
            var len = json.Length;
            JSONElement obj = new JSONElement(JSONElement.JSONType.DIC);
            JSONElement val;
            bool cle = true;
            while (index < len && json[index] != '}')
            {
                if (json[index] == ':')
                {
                    cle = false;
                    index++;
                }
                else if (json[index] == ',')
                {
                    cle = true;
                    index++;
                }
                else
                {
                    switch (GetJsonType(json[index]))
                    {
                        case JSONElement.JSONType.BOOL:
                            val = new JSONElement(JSONElement.JSONType.BOOL);
                            val.bool_value = ParseBool(json, ref index);
                            index++;
                            break;
                        case JSONElement.JSONType.STR:
                            if (cle)
                                obj.key.Add(ParseString(json, ref index));
                            else
                            {
                                val = new JSONElement(JSONElement.JSONType.STR);
                                val.string_value = ParseString(json, ref index);
                                obj.data.Add(val);
                            }
                            index++;
                            break;
                        case JSONElement.JSONType.NB:
                            val = new JSONElement(JSONElement.JSONType.NB);
                            val.int_value = ParseInt(json, ref index);
                            obj.data.Add(val);
                            break;
                        case JSONElement.JSONType.LIST:
                            val = new JSONElement(JSONElement.JSONType.LIST);
                            while (index < len && json[index] != ']')
                            {
                                EatBlank(json, ref index);
                                switch (GetJsonType(json[index]))
                                {
                                    case JSONElement.JSONType.BOOL:
                                        JSONElement b = new JSONElement(JSONElement.JSONType.BOOL);
                                        b.bool_value = ParseBool(json, ref index);
                                        val.data.Add(b);
                                        index++;
                                        break;
                                    case JSONElement.JSONType.STR:
                                        JSONElement s = new JSONElement(JSONElement.JSONType.STR);
                                        s.string_value = ParseString(json, ref index);
                                        val.data.Add(s);
                                        break;
                                    case JSONElement.JSONType.NB:
                                        JSONElement n = new JSONElement(JSONElement.JSONType.NB);
                                        n.int_value = ParseInt(json, ref index);
                                        val.data.Add(n);
                                        index++;
                                        break;
                                    case JSONElement.JSONType.DIC:
                                        JSONElement d = new JSONElement(JSONElement.JSONType.DIC);
                                        d = ParseJSONString(json, ref index);
                                        val.data.Add(d);
                                        break;
                                    default:
                                        index++;
                                        break;
                                }
                            }
                            obj.data.Add(val);
                            break;
                        case JSONElement.JSONType.DIC:
                            val = new JSONElement(JSONElement.JSONType.DIC);
                            index++;
                            val = ParseJSONString(json, ref index);
                            obj.data.Add(val);
                            break;
                        default:
                            val = new JSONElement(JSONElement.JSONType.NULL);
                            obj.data.Add(val);
                            break;
                    }
                }
            }
            index++;

            return obj;
        }

        public static JSONElement ParseJSONFile(string file)
        {
            string json = File.ReadAllText(file);
            int count = 1;
            return ParseJSONString(json, ref count);
        }

        public static string PrintJSON(JSONElement el)
        {
            string sortie = "";         
            switch (el.Type)
            {
                case JSONElement.JSONType.DIC:
                    sortie += "{";
                    var count = el.key.Count - 1;
                    for (int i = 0; i < count; i++)
                    {
                        sortie += el.key[i] + ":";
                        sortie += PrintJSON(el.data[i]);
                        sortie += ",";
                    }
                    sortie += el.key[count] + ":";
                    sortie += PrintJSON(el.data[count]);
                    sortie += "}";
                    break;
                case JSONElement.JSONType.BOOL:
                    sortie += el.bool_value;
                    break;
                case JSONElement.JSONType.NB:
                    sortie += el.int_value;
                    break;
                case JSONElement.JSONType.STR:
                    sortie += el.string_value;
                    break;
                case JSONElement.JSONType.LIST:
                    sortie += "[";
                    //Console.WriteLine(el.data.Count);
                    var countlist = el.data.Count - 1;
                    for (int i = 0; i < countlist; i++)
                    {
                        sortie += PrintJSON(el.data[i]);
                        sortie += ",";
                    }
                    sortie += PrintJSON(el.data[countlist]);
                    sortie += "]";
                    break;
                 default:
                    sortie += "PTDR nope";
                    break;
            }
            return sortie;
        }

        public static JSONElement SearchJSON(JSONElement element, string key)
        {
            for (int i = 0; i < element.key.Count; i++)
            {
                if (element.key[i] == key )
                {
                    return element.data[i];
                }
                else if (element.data[i].Type == JSONElement.JSONType.DIC)
                {
                    return SearchJSON(element.data[i], key);
                }
            }
            return null;
        }
    }
}
