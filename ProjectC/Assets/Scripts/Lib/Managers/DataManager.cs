using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string authenticationKey = "Server=192.168.219.132; Database=ProjectC; Uid=jww689741;Pwd =woo689741;";
    private SqlConnection database = new SqlConnection();

    // 데이터베이스 연결
    private SqlConnection ConnectAndOpenDatabase()
    {
        database.ConnectionString = authenticationKey;
        database.Open();
        return database;
    }
    
    // 단일 Attribute 데이터 조회
    public string SelectData(string select, string from, string where)
    {
        string returnValue = "";
        try
        {
            SqlCommand query = new SqlCommand();
            query.Connection = ConnectAndOpenDatabase();
            query.CommandText = $"SELECT {select} FROM {from} WHERE {where}"; // 쿼리문
            SqlDataReader result = query.ExecuteReader();
            while (result.Read())
            {
                returnValue = result.ToString();
            }
            database.Close();
            return returnValue;
        }
        catch (Exception e)
        {
            Debug.Log($"데이터베이스 접근중 오류가 발생했습니다 Exception : {e}");
            database.Close();
            return null;
        }
    }

    // 다중 Attribute 데이터 조회
    public List<string> SelectData(string[] select, string from, string where)
    {
        string _select = "";
        List<string> returnValue = new List<string>();
        try
        {
            SqlCommand query = new SqlCommand();
            query.Connection = ConnectAndOpenDatabase();
            for(int i = 0; i < select.Length; i++)
            {
                if(i > 0) _select += ", " + select[i];
                else _select += select[i];
            }
            query.CommandText = $"SELECT {_select} FROM {from} WHERE {where}"; // 쿼리문
            SqlDataReader result = query.ExecuteReader();
            while (result.Read())
            {
                for(int i = 0; i < select.Length; i++)
                {
                    returnValue.Add(result[i].ToString());
                }
            }
            database.Close();
            return returnValue;
        }
        catch(Exception e)
        {
            Debug.Log($"데이터베이스 접근중 오류가 발생했습니다 Exception : {e}");
            database.Close();
            return null;
        }
    }

    // 데이터 삽입
    public bool InsertData(string into, string values)
    {
        try
        {
            SqlCommand query = new SqlCommand();
            query.Connection = ConnectAndOpenDatabase();
            query.CommandText = $"INSERT INTO {into} VALUES {values}"; // 쿼리문
            query.ExecuteNonQuery();
            database.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"데이터베이스 접근중 오류가 발생했습니다 Exception : {e}");
            database.Close();
            return false;
        }
    }

    // 데이터 갱신
    public bool UpdateData(string update, string set, string where)
    {
        try
        {
            SqlCommand query = new SqlCommand();
            query.Connection = ConnectAndOpenDatabase();
            query.CommandText = $"UPDATE {update} SET {set} WHERE {where}"; // 쿼리문
            query.ExecuteNonQuery();
            database.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"데이터베이스 접근중 오류가 발생했습니다 Exception : {e}");
            database.Close();
            return false;
        }
    }

    // 데이터 삭제
    public bool DeleteData(string from, string where)
    {
        try
        {
            SqlCommand query = new SqlCommand();
            query.Connection = ConnectAndOpenDatabase();
            query.CommandText = $"DELETE FROM {from} WHERE {where}"; // 쿼리문
            query.ExecuteNonQuery();
            database.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"데이터베이스 접근중 오류가 발생했습니다 Exception : {e}");
            database.Close();
            return false;
        }
    }
}