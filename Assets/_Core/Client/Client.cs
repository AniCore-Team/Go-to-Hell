using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    private string client_name;

    public string Client_name => client_name;

    public void SetClient(string name)
    {
        client_name = name;
    }
}
