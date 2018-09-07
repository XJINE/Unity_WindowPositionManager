using System;
using System.Collections.Generic;

public static class CommandLineArgs
{
    #region Field

    private static string ToStringText;

    private static Dictionary<string, object> CommandLineArgsSet;

    #endregion Field

    #region Constructor

    static CommandLineArgs()
    {
        CommandLineArgsSet = new Dictionary<string, object>();
        ConstructCommandLineArgs();
    }

    #endregion Constructor

    #region Method

    private static void ConstructCommandLineArgs()
    {
        int index = 0;

        string[] commandLineArgsRaw = Environment.GetCommandLineArgs();

        while (index < commandLineArgsRaw.Length)
        {
            string arg = commandLineArgsRaw[index];

            if (arg.StartsWith("-"))
            {
                List<string> values = new List<string>();
                bool valuesKeep = false;

                index += 1;

                while (index < commandLineArgsRaw.Length)
                {
                    string value = commandLineArgsRaw[index];

                    if (value.StartsWith("-"))
                    {
                        break;
                    }
                    else
                    {
                        if (value.EndsWith(",")) 
                        {
                            value = value.TrimEnd(',');
                            valuesKeep = true;
                        }

                        values.Add(value);
                        index += 1;
                    }
                }

                if (!CommandLineArgsSet.ContainsKey(arg))
                {
                    if (values.Count == 0)
                    {
                        CommandLineArgsSet.Add(arg, null);
                    }
                    else if (values.Count == 1 && !valuesKeep)
                    {
                        CommandLineArgsSet.Add(arg, values[0]);
                    }
                    else 
                    {
                        CommandLineArgsSet.Add(arg, values.ToArray());
                    }
                }
            }
            else
            {
                index += 1;
            }
        }
    }

    public new static string ToString()
    {
        if (ToStringText == null) 
        {
            ToStringText = "";

            foreach (KeyValuePair<string, object> arg in CommandLineArgsSet)
            {
                ToStringText += arg.Key + " : ";

                if (arg.Value != null)
                {
                    if (arg.Value.GetType().IsArray)
                    {
                        object[] values = (object[])arg.Value;

                        foreach (object value in values)
                        {
                            ToStringText += (value == null ? " " : value.ToString()) + ", ";
                        }
                    }
                    else
                    {
                        ToStringText += arg.Value.ToString();
                    }
                }

                ToStringText += "\n";
            }
        }

        return ToStringText;
    }

    // NOTE:
    // To use "throw new Exception" is not so good
    // when undefined paramName is passed or failed to cast a value.
    // Exception is very hard to deal. This class should be used for easy development.

    public static bool GetValueAs<T>(string paramName, out T value)
    {
        // NOTE:
        // In most case, simple cast is failed.

        value = default(T);

        try
        {
            value = (T)CommandLineArgsSet[paramName];
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValueAsInt(string paramName, out int value) 
    {
        value = default(int);

        try
        {
            value = int.Parse((string)CommandLineArgsSet[paramName]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValueAsFloat(string paramName, out float value)
    {
        value = default(float);

        try
        {
            value = float.Parse((string)CommandLineArgsSet[paramName]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValueAsString(string paramName, out string value)
    {
        value = null;

        try
        {
            value = ((string)CommandLineArgsSet[paramName]);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValueAsBool(string paramName, out bool value) 
    {
        value = default(bool);

        try
        {
            // NOTE:
            // If the value is larger than 0, it becomes true.

            value = int.Parse((string)CommandLineArgsSet[paramName]) > 0;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValuesAsInt(string paramName, out int[] values) 
    {
        values = null;

        try
        {
            string[] valuesTemp = (string[])CommandLineArgsSet[paramName];

            values = new int[valuesTemp.Length];

            for (int i = 0; i < valuesTemp.Length; i++) 
            {
                values[i] = int.Parse(valuesTemp[i]);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValuesAsFloat(string paramName, out float[] values)
    {
        values = null;

        try
        {
            string[] valuesTemp = (string[])CommandLineArgsSet[paramName];

            values = new float[valuesTemp.Length];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = float.Parse(valuesTemp[i]);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValuesAsString(string paramName, out string[] values) 
    {
        values = null;

        try
        {
            values = (string[])CommandLineArgsSet[paramName];

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool GetValuesAsBool(string paramName, out bool[] values)
    {
        values = null;

        try
        {
            string[] valuesTemp = (string[])CommandLineArgsSet[paramName];

            values = new bool[valuesTemp.Length];

            for (int i = 0; i < valuesTemp.Length; i++)
            {
                values[i] = int.Parse(valuesTemp[i]) > 0;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool HasParameter(string paramName) 
    {
        return CommandLineArgsSet.ContainsKey(paramName);
    }

    #endregion Method
}