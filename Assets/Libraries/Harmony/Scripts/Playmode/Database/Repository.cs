using System.Collections.Generic;

namespace Harmony
{
    //Author: Mike Bédard
    public interface Repository <T>
    {
        T create(T item);
        T readById(int id);
        List<T> readAll();
        void update (T item);
        void delete (T item);
    }

}
