/*
 * Auteur Classe : Mike Bédard
 */
using System;

namespace Harmony
{
    public class StatisticTable
    {
        
        public static String SELECT_ALL = "" +
        "SELECT\n" +
        "statistic_id," +
        " seed," +
        " timestamp," +
        " bunny_quantity," +
        " fox_quantity," +
        " bunny_death_quantity," +
        " fox_death_quantity," +
        " bunny_hunger_death_quantity," +
        " bunny_thirst_death_quantity," +
        " bunny_eaten_death_quantity," +
        " fox_hunger_death_quantity," +
        " fox_thirst_death_quantity\n" +
        "FROM simulation_statistics";

        public static String SELECT_ALL_IDS = "" +
        "SELECT\n" +
        "statistic_id\n" +
        "FROM moments\n" +
        "ORDER BY id DESC";

        public static String SELECT_BY_ID = "" +
        "SELECT\n" +
        " statistic_id," +
        " seed," +
        " timestamp," +
        " bunny_quantity," +
        " fox_quantity," +
        " bunny_death_quantity," +
        " fox_death_quantity," +
        " bunny_hunger_death_quantity," +
        " bunny_thirst_death_quantity," +
        " bunny_eaten_death_quantity," +
        " fox_hunger_death_quantity," +
        " fox_thirst_death_quantity\n" +
        "FROM simulation_statistics WHERE id = ?";

        public static String INSERT = "" +
        "INSERT INTO simulation_statistics (\n" +
        "  seed,\n" +
        "  timestamp,\n" +
        "  bunny_quantity,\n" +
        "  fox_quantity,\n" +
        "  bunny_death_quantity,\n" +
        "  fox_death_quantity,\n" +
        "  bunny_hunger_death_quantity,\n" +
        "  bunny_thirst_death_quantity,\n" +
        "  bunny_eaten_death_quantity,\n" +
        "  fox_hunger_death_quantity,\n" +
        "  fox_thirst_death_quantity\n" +
        ") VALUES (\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?,\n" +
        "  ?\n" +
        ");";

        public static String UPDATE = "" +
        "UPDATE simulation_statistics\n" +
        "SET\n" +
        "  seed = ?,\n" +
        "  timestamp = ?,\n" +
        "  bunny_quantity = ?,\n" +
        "  fox_quantity = ?,\n" +
        "  bunny_death_quantity = ?,\n" +
        "  fox_death_quantity = ?,\n" +
        "  bunny_hunger_death_quantity = ?,\n" +
        "  bunny_thirst_death_quantity = ?,\n" +
        "  bunny_eaten_death_quantity = ?,\n" +
        "  fox_hunger_death_quantity = ?,\n" +
        "  fox_thirst_death_quantity = ?\n" +
        "WHERE statistic_id = ?;";

        public static String DELETE = "" +
        "DELETE FROM simulation_statistics\n" +
        "WHERE statistic_id = ?;";

        public static String COUNT = "" +
        "SELECT\n" +
        "COUNT(statistic_id)\n" +
        "FROM simulation_statistics";

        private StatisticTable() {
            // Private to create a static class
        }
    }
}