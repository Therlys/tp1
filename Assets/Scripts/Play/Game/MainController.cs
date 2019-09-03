using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private KeyCode timeScaleUpKey = KeyCode.KeypadPlus;
        [SerializeField] private KeyCode timeScaleDownKey = KeyCode.KeypadMinus;
        [SerializeField] private float timeScaleIncrement = 1;

        private void Awake()
        {
            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 125);
        }
        
        private void Start()
        {
            //TODO : Lire ce commentaire.
            //       Voici un exemple d'écriture sur la base de données en SQLite C#.
            //       N'oubliez pas de respecter les consignes de l'énoncé. Par exemple, ce code est loin
            //       d'être propre et il n'utilise pas le patron "Repository".
            var connection = Finder.SqLiteConnectionFactory.GetConnection();
            connection.Open();
            
            var transaction = connection.BeginTransaction();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO user(name,age) VALUES(?,?)";

            var userParameter = command.CreateParameter();
            userParameter.Value = "John Smith";
            command.Parameters.Add(userParameter);

            var ageParameter = command.CreateParameter();
            ageParameter.Value = 42;
            command.Parameters.Add(ageParameter);

            command.ExecuteNonQuery();
            transaction.Commit();
            
            connection.Close();
        }

        private void Update()
        {
            if (Input.GetKeyDown(timeScaleUpKey))
                Time.timeScale += timeScaleIncrement;
            if (Input.GetKeyDown(timeScaleDownKey))
                Time.timeScale -= timeScaleIncrement;
        }
    }
}