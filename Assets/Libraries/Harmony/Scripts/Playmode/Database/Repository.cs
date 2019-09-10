
using System.Collections.Generic;

/**
 * Interface pour un répositaire de commandes CRUD pour une base de données SQLite
 * @param <T> le type d'objet du modèle de donnée représentant une
 * une les données d'une ligne d'une table de la base de données
 */
public interface Repository <T>
{
        T create(T item);
        T readById(int id);
        List<T> readAll();
        void update (T item);
        void delete (T item);
}
