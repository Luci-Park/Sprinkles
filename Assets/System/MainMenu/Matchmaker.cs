using UnityEngine;

public static class Matchmaker
{
    const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789";
    static int codeLength = 7;
    
    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------
    public static string CreateCode()
    {
        string code = "";
        for (int i = 0; i < codeLength; i++)
        {
            code += glyphs[Random.Range(0, glyphs.Length)];
        }
        return code;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------


}
