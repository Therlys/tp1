﻿using System;
using UnityEngine;

namespace Harmony
{
    //TODO : Translate DOC to english.

    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les vecteurs à 2 dimensions de Unity.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Floor this vector.
        /// </summary>
        public static Vector2 AsFloored(this Vector2 value)
        {
            return new Vector2((int) value.x, (int) value.y);
        }

        /// <summary>
        /// Rounds this vector to the closets int.
        /// </summary>
        public static Vector2 AsRounded(this Vector2 value)
        {
            return new Vector2(value.x.RoundToInt(), value.y.RoundToInt());
        }

        /// <summary>
        /// Returns the direction of point1 to point2.
        /// </summary>
        public static Vector2 DirectionTo(this Vector2 point1, Vector2 point2)
        {
            return point2 - point1;
        }
        
        /// <summary>
        /// Indique la distance entre deux points.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <returns>Distance entre les deux points.</returns>
        public static float DistanceTo(this Vector2 point1, Vector2 point2)
        {
            return (point2 - point1).magnitude;
        }

        /// <summary>
        /// Indique la distance entre deux points, au carré.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <returns>Distance entre les deux points, au carré.</returns>
        public static float SqrDistanceTo(this Vector2 point1, Vector2 point2)
        {
            return (point2 - point1).sqrMagnitude;
        }

        /// <summary>
        /// Indique si un point est proche d'un autre point en fonction d'une distance maximale.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <param name="maxDistance">Distance maximale entre les deux points.</param>
        /// <returns>Vrai si le "point1" est proche du "point2".</returns>
        public static bool IsCloseOf(this Vector2 point1, Vector2 point2, float maxDistance)
        {
            if (maxDistance < 0)
            {
                throw new ArgumentException(nameof(maxDistance) + " must be greater or equal to 0.");
            }

            return point1.SqrDistanceTo(point2) < maxDistance * maxDistance;
        }

        /// <summary>
        /// Permet de savoir si deux vecteurs de direction sont dans la même direction.
        /// </summary>
        /// <param name="direction1">Point de début de la direction.</param>
        /// <param name="direction2">Point de fin de la direction.</param>
        /// <param name="precision">
        /// Valeur de précision. À 0, l'angle maximal entre les deux vecteurs est de 90. À 1, l'angle maximal entre les deux vecteurs est de 0.
        /// La valeur de précision peut être calculée avec le Cosinus de l'angle maximal (en degrées). La valeur par défaut est de 1.
        /// </param>
        /// <returns>Vrai si "direction1" et "direction2" sont dans la même direction.</returns>
        public static bool IsSameDirection(this Vector2 direction1, Vector2 direction2, float precision = 1f)
        {
            if (precision < 0 || precision > 1)
            {
                throw new ArgumentException("Precision must be between 0 and 1, inclusive.");
            }

            return Vector2.Dot(direction1.normalized, direction2.normalized) >= precision;
        }

        /// <summary>
        /// Permet d'obtenir le point sur une ligne le plus proche d'un autre point.
        /// </summary>
        /// <remarks>
        /// Le point donné est toujours entre "from" et "to".
        /// </remarks>
        /// <param name="from">Position de début de la ligne.</param>
        /// <param name="to">Position de find de la ligne.</param>
        /// <param name="position">Position d'où débuter la recherche.</param>
        /// <returns>Point, sur la ligne entre "from" et "to, le plus proche de "position".</returns>
        public static Vector2 ClosestPointOnLine(Vector2 from, Vector2 to, Vector2 position)
        {
            var directionToLine = position - from;
            var lineDirectionNormalized = (to - from).normalized;

            var lineLength = Vector2.Distance(from, to);
            var travelDistance = Vector2.Dot(directionToLine, lineDirectionNormalized);

            if (travelDistance <= 0) return from;
            if (travelDistance >= lineLength) return to;

            var travelDirection = lineDirectionNormalized * travelDistance;
            var closestPoint = from + travelDirection;

            return closestPoint;
        }

        /// <summary>
        /// Effectue la rotation d'un point avec des angles en degrés.
        /// </summary>
        /// <param name="point">Point ayant à subir la rotation.</param>
        /// <param name="angle">Angle de rotation en degrés sur l'axe des X.</param>
        /// <returns>Nouveau point avec la rotation souhaité.</returns>
        public static Vector2 Rotate(this Vector2 point, float angle)
        {
            angle = Mathf.Deg2Rad * angle;

            float angleCos = Mathf.Cos(angle);
            float angleSin = Mathf.Sin(angle);

            return new Vector2(point.x * angleCos - point.y * angleSin,
                               point.x * angleSin + point.y * angleCos);
        }

        /// <summary>
        /// Effectue la rotation d'un point autour d'un pivot avec des angles en degrés.
        /// </summary>
        /// <param name="point">Point ayant à subir la rotation.</param>
        /// <param name="pivot">Pivot de rotation.</param>
        /// <param name="angle">Angle de rotation en degrés sur l'axe des X.</param>
        /// <returns>Nouveau point avec la rotation souhaité.</returns>
        public static Vector2 RotateAround(this Vector2 point, Vector2 pivot, float angle)
        {
            return (point - pivot).Rotate(angle) + pivot;
        }
    }
}