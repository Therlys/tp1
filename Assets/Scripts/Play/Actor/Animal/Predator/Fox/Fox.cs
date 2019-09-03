using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public sealed class Fox : Animal, IPredator
    {
        private bool IsMoving = false;

        private List<Node> nodes = null;
        //TODO : Compléter le comportement du renard.
        private void Update()
        {
            if (Sensor.SensedObjects.Count == 0)
            {
                Mover.MoveTo(PathFinder.FindRandomWalk(Position, 4).ElementAt(0).Position3D);
            }
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var bunny = sensedObject.GetComponent<Bunny>();
                if (!IsMoving && bunny != null)
                {

                    nodes = PathFinder.FindPath(Position, bunny.Position);
                    StartCoroutine(Move());
                    // Je suis un @$@!@ de lapin
                    //bunny.Eat();
                    break;
                }
            }
        }

        private IEnumerator Move()
        {
            IsMoving = true;

            foreach (var node in nodes)
            {
                Mover.MoveTo(node.Position3D);
                yield return new WaitForSeconds(1.0f);
            }

            IsMoving = false;
            nodes = null;
        }

        private void OnDrawGizmos()
        {
            if(nodes == null) return;

            for (int i = 1; i < nodes.Count; i++)
            {
                GizmosExtensions.DrawArrow(nodes.ElementAt(i-1).Position3D, nodes.ElementAt(i).Position3D, Color.green);
            }
        }
    }
}