using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public static class Strings
{

    #region fields


    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    static Dictionary<string, string> foreign_characters = new Dictionary<string, string>
    {
        { "äæǽ", "ae" },
        { "öœ", "oe" },
        { "ü", "u" },
        { "Ä", "a" },
        { "Ü", "u" },
        { "Ö", "o" },
        { "Ё", "e" },
        { "ё", "e" },
        { "é", "e" },
        { "è", "e" },
        { "ê", "e" },
        { "à", "a" },
        { "â", "a" },
        { "î", "i" },
        { "ï", "i" },
        { "û", "u" },
        { "ô", "o" },
        { "ö", "o" },
        { "-", " " }
    };


    public static string RemoveDiacritics(this string s)
    {
        string text = "";

        foreach (char c in s)
        {
            int len = text.Length;

            foreach (KeyValuePair<string, string> entry in foreign_characters)
            {
                if (entry.Key.IndexOf(c) != -1)
                {
                    text += entry.Value;
                    break;
                }
            }

            if (len == text.Length)
            {
                text += c;
            }
        }
        return text;
    }

    #endregion
}

public class QuestionCollection : MonoBehaviour
{

    #region fields

    private TextAsset file;
    private XmlDocument xmlDoc;
    private QuizQuestion[] _ListOfQuestions;
    private QuizQuestion[] allQuestions_CultureGenerale;
    private QuizQuestion[] allQuestions_Histoire;
    private QuizQuestion[] allQuestions_Geographie;
    private QuizQuestion[] allQuestions_SciencesNature;
    private QuizQuestion[] allQuestions_Medecine;
    private QuizQuestion[] allQuestions_HarryPotter;
    private QuizQuestion[] allQuestions_StarWars;
    private QuizQuestion[] allQuestions_ESport;
    private QuizQuestion[] allQuestions_Nintendo;
    private QuizQuestion[] allQuestions_Playstation;
    private QuizQuestion[] allQuestions_Disney;
    private QuizQuestion[] allQuestions_Mathematiques;
    private QuizQuestion[] allQuestions_Japon;
    private QuizQuestion[] allQuestions_USA;
    private QuizQuestion[] allQuestions_Mythologie;
    private QuizQuestion[] allQuestions_SeriesTV;
    private QuizQuestion[] allQuestions_LesSimpsons;
    private QuizQuestion[] allQuestions_Cinema;
    private QuizQuestion[] allQuestions_BattleRoyale;
    private QuizQuestion[] allQuestions_Fitness;
    private QuizQuestion[] allQuestions_Pokemon;
    private QuizQuestion[] allQuestions_Retrogaming;
    private QuizQuestion[] allQuestions_PopCulture;
    private QuizQuestion[] allQuestions_Marvel;
    private QuizQuestion[] allQuestions_Divertissements;
    private QuizQuestion[] allQuestions_SportsLoisirs;
    private QuizQuestion[] allQuestions_ArtsLitterature;
    private QuizQuestion[] allQuestions_Economie;
    private QuizQuestion[] allQuestions_Xbox;
    private QuizQuestion[] allQuestions_Football;
    private QuizQuestion[] allQuestions_Animaux;
    private QuizQuestion[] allQuestions_Musique;
    private QuizQuestion[] allQuestions_Mario;
    private QuizQuestion[] allQuestions_Annees80;
    private QuizQuestion[] allQuestions_Societe;
    private QuizQuestion[] allQuestions_Informatique;
    private QuizQuestion[] allQuestions_Couture;
    private QuizQuestion[] allQuestions_Insolite;
    private QuizQuestion[][] allQuestions;


    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    private void Awake()
    {
        xmlDoc = new XmlDocument();

        StartCoroutine(LoadQuestionsCo());
    }

    /// <summary>
    /// Checks if questions file exists and loads questions.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadQuestionsCo()
    {
        yield return new WaitForSeconds(.5f);

        file = Resources.Load("Questions", typeof(TextAsset)) as TextAsset;

        if (file == null)
        {
            Debug.LogError("'Questions.xml' file is missing.");
        }

        LoadAllQuestions();

    }

    /// <summary>
    /// Loads all of the questions in the "Questions.txt" file.
    /// </summary>
    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[][]));
        if (file != null)
        {
            xmlDoc.LoadXml(file.text);
        }

        using (TextReader streamReader = new StringReader(xmlDoc.InnerXml))
        {
            allQuestions = (QuizQuestion[][])serializer.Deserialize(streamReader);
        }

        allQuestions_Geographie = allQuestions[0];
        allQuestions_CultureGenerale = allQuestions[1];
        allQuestions_Histoire = allQuestions[2];
        allQuestions_Divertissements = allQuestions[3];
        allQuestions_SciencesNature = allQuestions[4];
        allQuestions_ArtsLitterature = allQuestions[5];
        allQuestions_SportsLoisirs = allQuestions[6];
        allQuestions_Mathematiques = allQuestions[7];
        allQuestions_USA = allQuestions[8];
        allQuestions_Japon = allQuestions[9];
        allQuestions_Disney = allQuestions[10];
        allQuestions_Medecine = allQuestions[11];
        allQuestions_HarryPotter = allQuestions[12];
        allQuestions_Marvel = allQuestions[13];
        allQuestions_StarWars = allQuestions[14];
        allQuestions_ESport = allQuestions[15];
        allQuestions_Nintendo = allQuestions[16];
        allQuestions_Playstation = allQuestions[17];
        allQuestions_Mythologie = allQuestions[18];
        allQuestions_Cinema = allQuestions[19];
        allQuestions_LesSimpsons = allQuestions[20];
        allQuestions_SeriesTV = allQuestions[21];
        allQuestions_BattleRoyale = allQuestions[22];
        allQuestions_Fitness = allQuestions[23];
        allQuestions_Pokemon = allQuestions[24];
        allQuestions_Retrogaming = allQuestions[25];
        allQuestions_PopCulture = allQuestions[26];
        allQuestions_Economie = allQuestions[27];
        allQuestions_Xbox = allQuestions[28];
        allQuestions_Football = allQuestions[29];
        allQuestions_Animaux = allQuestions[30];
        allQuestions_Musique = allQuestions[31];
        allQuestions_Mario = allQuestions[32];
        allQuestions_Annees80 = allQuestions[33];
        allQuestions_Societe = allQuestions[34];
        allQuestions_Informatique = allQuestions[35];
        allQuestions_Couture = allQuestions[36];
        allQuestions_Insolite = allQuestions[37];
    }

    /// <summary>
    /// Returns one new question from one category.
    /// </summary>
    /// <param name="_category"></param>
    /// <returns></returns>
    public QuizQuestion GetUnaskedQuestion(string _category)
    {
        QuizQuestion[] _ListOfQuestions = GetQuestionsFromCategory(_category);
        ResetQuestionsIfAllHaveBeenAsked(_ListOfQuestions);

        QuizQuestion question = _ListOfQuestions
            .Where(t => t.Asked == false)
            .OrderBy(t => UnityEngine.Random.Range(0, int.MaxValue))
            .FirstOrDefault();

        question.Asked = true;
        SerializeQuestions();
        return question;
    }

    /// <summary>
    /// Checks if all questions have been asked.
    /// </summary>
    /// <param name="questions"></param>
    private void ResetQuestionsIfAllHaveBeenAsked(QuizQuestion[] questions)
    {
        if (questions.Any(t => t.Asked == false) == false)
        {
            ResetQuestions(questions);
        }
    }

    /// <summary>
    /// Sets all questions asked boolean to false.
    /// </summary>
    /// <param name="questions"></param>
    private void ResetQuestions(QuizQuestion[] questions)
    {
        foreach (var question in questions)
        {
            question.Asked = false;
        }
    }

    /// <summary>
    /// Saves questions into "Questions.txt" file.
    /// </summary>
    public void SerializeQuestions()
    {
        allQuestions = new QuizQuestion[][] { allQuestions_Geographie, allQuestions_CultureGenerale, allQuestions_Histoire, allQuestions_Divertissements, allQuestions_SciencesNature,
            allQuestions_ArtsLitterature,allQuestions_SportsLoisirs, allQuestions_Mathematiques, allQuestions_USA, allQuestions_Japon, allQuestions_Disney,
            allQuestions_Medecine, allQuestions_HarryPotter, allQuestions_Marvel, allQuestions_StarWars, allQuestions_ESport, allQuestions_Nintendo,
            allQuestions_Playstation, allQuestions_Mythologie, allQuestions_Cinema, allQuestions_LesSimpsons, allQuestions_SeriesTV, allQuestions_BattleRoyale,
            allQuestions_Fitness, allQuestions_Pokemon, allQuestions_Retrogaming, allQuestions_PopCulture,allQuestions_Economie,allQuestions_Xbox,allQuestions_Football,allQuestions_Animaux,allQuestions_Musique,
            allQuestions_Mario, allQuestions_Annees80, allQuestions_Societe, allQuestions_Informatique, allQuestions_Couture, allQuestions_Insolite};


        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[][]));
        using (StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/Resources/Questions.xml"))
        {
            serializer.Serialize(streamWriter, allQuestions);
        }

    }

    /// <summary>
    /// Sets the list of questions corresponding to one category.
    /// </summary>
    /// <param name="_category"></param>
    /// <returns></returns>
    private QuizQuestion[] GetQuestionsFromCategory(string _category)
    {
        switch (_category)
        {
            case "Culture Générale":
                return allQuestions_CultureGenerale;
            case "Histoire":
                return allQuestions_Histoire;
            case "Géographie":
                return allQuestions_Geographie;
            case "Sciences & Nature":
                return allQuestions_SciencesNature;
            case "Santé":
                return allQuestions_Medecine;
            case "Harry Potter":
                return allQuestions_HarryPotter;
            case "Star Wars":
                return allQuestions_StarWars;
            case "Nintendo":
                return allQuestions_Nintendo;
            case "eSport":
                return allQuestions_ESport;
            case "Playstation":
                return allQuestions_Playstation;
            case "Disney":
                return allQuestions_Disney;
            case "Mathématiques":
                return allQuestions_Mathematiques;
            case "U.S.A.":
                return allQuestions_USA;
            case "Japon":
                return allQuestions_Japon;
            case "Mythologie":
                return allQuestions_Mythologie;
            case "Séries TV":
                return allQuestions_SeriesTV;
            case "Les Simpsons":
                return allQuestions_LesSimpsons;
            case "Cinéma":
                return allQuestions_Cinema;
            case "Battle Royale":
                return allQuestions_BattleRoyale;
            case "Fitness":
                return allQuestions_Fitness;
            case "Pokémon":
                return allQuestions_Pokemon;
            case "Retrogaming":
                return allQuestions_Retrogaming;
            case "Pop Culture":
                return allQuestions_PopCulture;
            case "Marvel":
                return allQuestions_Marvel;
            case "Divertissements":
                return allQuestions_Divertissements;
            case "Sports & Loisirs":
                return allQuestions_SportsLoisirs;
            case "Arts & Littérature":
                return allQuestions_ArtsLitterature;
            case "Economie":
                return allQuestions_Economie;
            case "Xbox":
                return allQuestions_Xbox;
            case "Football":
                return allQuestions_Football;
            case "Animaux":
                return allQuestions_Animaux;
            case "Musique":
                return allQuestions_Musique;
            case "Mario":
                return allQuestions_Mario;
            case "Années 80":
                return allQuestions_Annees80;
            case "Société":
                return allQuestions_Societe;
            case "Informatique":
                return allQuestions_Informatique;
            case "Couture":
                return allQuestions_Couture;
            case "Insolite":
                return allQuestions_Insolite;
            default:
                return null;
        }
    }

    #endregion
}
