using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;


public class StatisticRepository : Repository<Statistic>
{
    
    private DbConnection dbConnection;

    public StatisticRepository(DbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    public Statistic create(Statistic statistic)
    {
        dbConnection.Open();

        if (statistic == null) return null;
        var transaction = dbConnection.BeginTransaction();
        try {
            var command = dbConnection.CreateCommand();
            command.CommandText = StatisticTable.INSERT;
            AddStatisticExceptIdToCommand(command, statistic);
            command.ExecuteNonQuery();
            var idGetCommand = dbConnection.CreateCommand();
            idGetCommand.CommandText = "SELECT last_insert_rowid()";
            var reader = idGetCommand.ExecuteReader();
            int id = 0;
            while(reader.Read())
            {
                id = reader.GetInt32(0);
            }
            statistic.StatisticId = id;
        } catch (Exception e)
        {
            #if UNITY_EDITOR
            Debug.Log(e);
            #endif
            return null;
        } 
        transaction.Commit();
        dbConnection.Close();
        return statistic;
    }

    public Statistic readById(int id)
    {
        dbConnection.Open();
        Statistic statistic = null;
        var transaction = dbConnection.BeginTransaction();
        try {
            var command = dbConnection.CreateCommand();
            command.CommandText = StatisticTable.SELECT_BY_ID;
            AddValueParameterToCommand(command, id);
            statistic = GetStatisticsAssociatedWithCommand(command)[0];
        } catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e);
#endif
            return null;
        } 
        transaction.Commit();
        dbConnection.Close();
        return statistic;
    }


    public List<Statistic> readAll()
    {
        dbConnection.Open();
        List<Statistic> statistics = null;
        var transaction = dbConnection.BeginTransaction();
        try {
            var command = dbConnection.CreateCommand();
            command.CommandText = StatisticTable.SELECT_ALL;
            statistics = GetStatisticsAssociatedWithCommand(command);
        } catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e);
#endif
            return null;
        } 
        transaction.Commit();
        dbConnection.Close();
        return statistics;
    }

    public void update(Statistic statistic)
    {
        dbConnection.Open();
        var transaction = dbConnection.BeginTransaction();
        try {
            var command = dbConnection.CreateCommand();
            command.CommandText = StatisticTable.UPDATE;
            AddStatisticExceptIdToCommand(command, statistic);
            AddValueParameterToCommand(command, statistic.StatisticId);
            command.ExecuteNonQuery();
        } catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e);
#endif
            return;
        } 
        transaction.Commit();
        dbConnection.Close();
    }

    public void delete(Statistic statistic)
    {
        dbConnection.Open();
        var transaction = dbConnection.BeginTransaction();
        try {
            var command = dbConnection.CreateCommand();
            command.CommandText = StatisticTable.DELETE;
            AddValueParameterToCommand(command, statistic.StatisticId);
            command.ExecuteNonQuery();
        } catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e);
#endif
            return;
        } 
        transaction.Commit();
        dbConnection.Close();
    }

    private void AddStatisticExceptIdToCommand(DbCommand command, Statistic statistic)
    {
        AddValueParameterToCommand(command, statistic.Seed);
        AddValueParameterToCommand(command, statistic.Timestamp);
        AddValueParameterToCommand(command, statistic.BunnyQuantity);
        AddValueParameterToCommand(command, statistic.FoxQuantity);
        AddValueParameterToCommand(command, statistic.BunnyDeathQuantity);
        AddValueParameterToCommand(command, statistic.FoxDeathQuantity);
        AddValueParameterToCommand(command, statistic.BunnyHungerDeathQuantity);
        AddValueParameterToCommand(command, statistic.BunnyThirstDeathQuantity);
        AddValueParameterToCommand(command, statistic.BunnyEatenDeathQuantity);
        AddValueParameterToCommand(command, statistic.FoxHungerDeathQuantity);
        AddValueParameterToCommand(command, statistic.FoxThirstDeathQuantity);
    }
    private void AddValueParameterToCommand(DbCommand command, object value)
    {
        var parameter = command.CreateParameter();
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
    private List<Statistic> GetStatisticsAssociatedWithCommand(DbCommand command)
    {
        var reader = command.ExecuteReader();
        List<Statistic> statistics = new List<Statistic>();
        while(reader.Read())
        {
            Statistic currentStatistic = new Statistic();
            currentStatistic.Seed = reader.GetInt32(1);
            currentStatistic.Timestamp = reader.GetInt32(2);
            currentStatistic.BunnyQuantity = reader.GetInt32(3);
            currentStatistic.FoxQuantity = reader.GetInt32(4);
            currentStatistic.BunnyDeathQuantity = reader.GetInt32(5);
            currentStatistic.FoxDeathQuantity = reader.GetInt32(6);
            currentStatistic.BunnyHungerDeathQuantity = reader.GetInt32(7);
            currentStatistic.BunnyThirstDeathQuantity = reader.GetInt32(8);
            currentStatistic.BunnyEatenDeathQuantity = reader.GetInt32(9);
            currentStatistic.FoxHungerDeathQuantity = reader.GetInt32(10);
            currentStatistic.FoxThirstDeathQuantity = reader.GetInt32(11);
            statistics.Add(currentStatistic);
        }

        return statistics;
    }
}
