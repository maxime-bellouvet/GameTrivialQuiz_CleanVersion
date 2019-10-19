using System.Linq;

public class UICommon
{
    
    #region fields


    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    public int[] ChooseRandomIndices(int maxIndex)
    {
        int[] randomIndices = new int[maxIndex];
        int[] indices = new int[maxIndex];
        for (int i = 0; i < maxIndex; i++)
        {
            indices[i] = i;
        }
        System.Random rnd = new System.Random();
        randomIndices = indices.OrderBy(x => rnd.Next()).ToArray();
        return randomIndices;
    }

    public int FixFontSizeForQuestion(int textLength)
    {
        if (textLength > 280)
        {
            return 42;
        }
        else if (textLength > 176)
        {
            return 50;
        }
        else if (textLength > 112)
        {
            return 62;
        }
        else
        {
            return 72;
        }
    }

    public int FixFontSizeForAnswers(int textLength)
    {
        if (textLength > 296)
        {
            return 27;
        }
        else if (textLength > 160)
        {
            return 30;
        }
        else if (textLength > 104)
        {
            return 40;
        }
        else if (textLength > 72)
        {
            return 50;
        }
        else if (textLength > 56)
        {
            return 55;
        }
        else
        {
            return 70;
        }
    }

    #endregion
}
