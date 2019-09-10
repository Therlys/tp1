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

        private Animal recurTarget = null;
        
        private Coroutine routineForMoving;
        private Coroutine moveToRoutine;
        
        private bool stopping;
        private List<Node> nodes = null;
        
        public bool IsFollowingPath => nodes != null;

        private string debugStateTag;
        protected PathFinder PathFinder => pathFinder;
        public VitalStats Vitals => vitals;
        protected Mover Mover => mover;
        protected Feeder Feeder => feeder;
        //               OffspringCreator :  Sert à reproduire l'animal. Indiquez lui la cible et l'animal se reproduira avec
        //                                   cette cible si ladite cible est à portée. Lorsque l'animal se reproduit, ses signes
        //                                   vitaux s'améliorent. Notez que l'animal ne peut pas mourrir d'une carence en ****.
        protected OffspringCreator OffspringCreator => offspringCreator;
        protected Sensor Sensor => sensor;

        public bool IsHungry => vitals.Hunger > hungerThreshold;
        public bool IsThirsty => vitals.Thirst > thirstThreshold;
        public bool IsHorny => vitals.ReproductiveUrge > reproductiveUrgeThreshold;
        public bool IsDead => vitals.IsDead;
        public bool IsAvailable => recurTarget == null || !IsRecurring;

        public bool IsRecurring = false;

        protected void Awake()
        {
            pathFinder = Finder.PathFinder;
            vitals = GetComponentInChildren<VitalStats>();
            mover = GetComponentInChildren<Mover>();
            feeder = GetComponentInChildren<Feeder>();
            offspringCreator = GetComponentInChildren<OffspringCreator>();
            sensor = GetComponentInChildren<Sensor>();
            stateMachine = new StateMachine(this);
            Vitals.OwnerType = this.GetType();
            if(Vitals.OwnerType == typeof(Game.Fox))
            Debug.Log("Fox : " + (Vitals.OwnerType == typeof(Fox)));
        }

        public bool AskToRecur(Animal recurTarget)
        {
            if (recurTarget != null) return false;
            else
            {
                this.recurTarget = recurTarget;
                return true;
            }
        }

        public void StopRecurring(Animal recurTarget)
        {
            if (this.recurTarget == recurTarget) recurTarget = null;
            
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
        
        public IDrinkable GetNearestDrinkable()
        {
            IDrinkable drinkable = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var sensedDrinkable = sensedObject.GetComponent<IDrinkable>();
                if (sensedDrinkable != null)
                {
                    if (drinkable == null || MathExtensions.SquareDistanceBetween(Position, sensedDrinkable.Position) < MathExtensions.SquareDistanceBetween(Position, drinkable.Position))
                    {
                        drinkable = sensedDrinkable;
                    }
                }
            }
            return drinkable;
        }
#if UNITY_EDITOR
        public void SetDebugStateTag(string debugStateTag)
        {
            this.debugStateTag = debugStateTag;
        }
#endif
        

        public abstract IPredator GetNearestPredator();

        public abstract Animal GetNearestFriend();

        public abstract bool IsBeingHunted();
       

        public void GoAwayFrom(Vector3? fleeingPosition)
        {
            //Le node peut être null dû au fait que le lapin peut se retrouver pris dans un endroit qui ne le permet pas de s'enfuir
            //Si aucun endroit n'est trouvé pour s'enfuir, cette méthode-ci retourne nulle, donc il faut mettre le node dans une variable
            //et mettre une condition pour savoir si le node n'est pas null
            Node node = PathFinder.FindFleePath(Position, (Vector3) fleeingPosition);
            if(node != null) MoveTo(node.Position3D);
        }



        public void MoveTo(Vector3? destination)
        {
            if(!IsAvailable) return;
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
        
        public bool Drink(IDrinkable drinkable)
        {
            return feeder.Drink(drinkable);
        }

        public bool CreateOffspringWith(Animal friend)
        {
            friend.StopRecurring(this);
            return offspringCreator.CreateOffspringWith(friend);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (debugStateTag != null) GizmosExtensions.DrawText(Position, debugStateTag);
            if (nodes != null) GizmosExtensions.DrawPath(nodes);
        }
#endif
    }
}