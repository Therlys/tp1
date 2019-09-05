using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Animal : Actor
    {
        [Header("Vitals")] [SerializeField] private float hungerThreshold = 0.8f;
        [SerializeField] private float thirstThreshold = 0.8f;
        [SerializeField] private float reproductiveUrgeThreshold = 0.8f;

        private PathFinder pathFinder;
        private VitalStats vitals;
        private Mover mover;
        private Feeder feeder;
        private OffspringCreator offspringCreator;
        private Sensor sensor;
        private StateMachine stateMachine;
        private Coroutine moveToRoutine;
        private Coroutine moveAwayFromRoutine;
        private bool stopping;
        private List<Node> nodes = null;
        public bool IsFollowingPath => nodes != null;

        /*private List<Node> Nodes
        {
            get { return nodes; }

            set
            {
                if (value == null) nodes = null;
                foreach (var node in value)
                    {
                        if (nodes == null || !nodes.Contains(node))
                        {
                            nodes = value;
                            StartCoroutine(StopMoveToRoutine());
                            break;
                        }
                    }
            }
        }*/

        //               PathFinder :        Outil de recherche de chemin sur un graphe. Permet de trouver un
        //                                   chemin d'un point A à un point B. Possède aussi d'autres méthodes
        //                                   de recherche de chemin, tels que la recherche d'un chemin aléatoire
        //                                   ou de chemin de fuite. Ces deux derniers algorithmes sont basiques
        //                                   et peu efficaces; vous pouvez les améliorer si désiré.
        protected PathFinder PathFinder => pathFinder;
        //               Vitals :            Représente les signes vitaux de l'animal. Si les signes vitaux sont critiques,
        //                                   l'animal meurt automatiquement. Le but de votre machine à état sera de conserver
        //                                   votre animal en vie malgré la baisse constante de ces signes vitaux.
        public VitalStats Vitals => vitals;
        //               Mover :             Sert à déplacer l'animal d'un point A à un point B. Il ne fait aucune
        //                                   recherche de chemin. Il est donc de votre responsabilité de déplacer l'animal de point
        //                                   en point sur le graphe (voir PathFinder).
        protected Mover Mover => mover;
        //               Feeder :            Sert à nourir l'animal. Indiquez lui la cible et l'animal s'en nourira ladite
        //                                   cible est à portée. Lorsque l'animal se nouri, ses signes vitaux s'améliorent.
        protected Feeder Feeder => feeder;
        //               OffspringCreator :  Sert à reproduire l'animal. Indiquez lui la cible et l'animal se reproduira avec
        //                                   cette cible si ladite cible est à portée. Lorsque l'animal se reproduit, ses signes
        //                                   vitaux s'améliorent. Notez que l'animal ne peut pas mourrir d'une carence en ****.
        protected OffspringCreator OffspringCreator => offspringCreator;
        //               Sensor :            Sert à lire l'environnement autour de l'animal. Utilisez le pour détecter les sources
        //                                   de nouriture, d'eau et même les prédateurs.
        protected Sensor Sensor => sensor;

        public bool IsHungry => vitals.Hunger > hungerThreshold;
        public bool IsThirsty => vitals.Thirst > thirstThreshold;
        public bool IsHorny => vitals.ReproductiveUrge > reproductiveUrgeThreshold;
        public bool IsDead => vitals.IsDead;

        protected void Awake()
        {
            pathFinder = Finder.PathFinder;
            vitals = GetComponentInChildren<VitalStats>();
            mover = GetComponentInChildren<Mover>();
            feeder = GetComponentInChildren<Feeder>();
            offspringCreator = GetComponentInChildren<OffspringCreator>();
            sensor = GetComponentInChildren<Sensor>();
            stateMachine = new StateMachine(this);
        }

        private void Update()
        {
#if UNITY_EDITOR
            try
            {
#endif
                stateMachine.Update();
#if UNITY_EDITOR
            }
            catch (Exception ex)
            {
                Debug.LogError($"{name} errored : {ex.Message}\n{ex.StackTrace}.", gameObject);
                gameObject.SetActive(false);
            }
#endif
        }

        private void OnEnable()
        {
            vitals.OnDeath += OnDeath;

            if (vitals.IsDead) OnDeath();
        }

        private void OnDisable()
        {
            vitals.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Destroy();
        }

        [ContextMenu("Destroy")]
        private void Destroy()
        {
            Destroy(gameObject);
        }

        public abstract IEatable GetNearestEatable();

        public virtual IPredator GetNearestPredator()
        {
            return null;
        }



        public void MoveTo(Vector3? destination)
        {
            if (stopping && moveToRoutine != null)
            {
                StopCoroutine(moveToRoutine);
            }
            moveToRoutine = StartCoroutine(MoveToRoutine(destination));
        }


        private IEnumerator FollowPathRoutine()
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    mover.MoveTo(node.Position3D);
                    yield return new WaitForSeconds(1.0f);
                }
            }

            nodes = null;
        }


        private IEnumerator MoveToRoutine(Vector3? destination)
        {
            stopping = true;
            while (routineForMoving != null)
            {
                if (!mover.IsMoving)
                {
                    StopCoroutine(routineForMoving);
                    nodes = null;
                    routineForMoving = null;
                }
                yield return null;
            }
            nodes = destination == null ? pathFinder.FindRandomWalk(Position, 10) : pathFinder.FindPath(Position, (Vector3) destination);
            routineForMoving = StartCoroutine(FollowPathRoutine());
            stopping = false;
        }

        public bool Eat(IEatable eatable)
        {
            return feeder.Eat(eatable);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            //TODO : Pour faciliter le déboguage, affichez des informations dans l'onglet "Scene" via la classe "GizmosExtensions".
        }
#endif
    }
}