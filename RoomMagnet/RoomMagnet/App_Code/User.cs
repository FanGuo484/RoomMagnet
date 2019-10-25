﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>
public class User
{
    private string firstName;
    private string lastName;
    private string middleName;
    private string email;
    private string passWord;
    private DateTime birthday;
    private string backgroundURL;
    private DateTime lastUpdated;
    private string lastUpdatedBy;
    public static User[] Users = new User[1000];
    public static int UserCount = 0;

    public User(string firstName, string LastName, string email, string passWord, DateTime birthday)
    {
        setFirstName(firstName);
        setLastName(lastName);
        setMiddleName(middleName);
        setEmail(email);
        setPassword(passWord);
        setBirthday(birthday);
        setLastupdated(DateTime.Now);
        setBy("Room Magnet"); 
        UserCount++;

    }

    public void setFirstName(string firstName)
    {
        this.firstName = firstName;
    }

    public string getFirstName()
    {
        return this.firstName;
    }

    public void setLastName(string lastName)
    {
        this.lastName = lastName;
    }

    public string getLastName()
    {
        return this.lastName;
    }

    public void setMiddleName(string middleName)
    {
        this.middleName = middleName;
    }

    public string getMiddleName()
    {
        return this.middleName;
    }

    public void setEmail(string email)
    {
        this.email = email;
    }

    public string getEmail()
    {
        return this.email;
    }

    public void setPassword(string passWord)
    {
        this.passWord = passWord;
    }

    public string getPassword()
    {
        return this.passWord;
    }

    public void setBirthday(DateTime birthday)
    {
        this.birthday = birthday;
    }

    public DateTime getBirthday()
    {
        return this.birthday;
    }

    public void setURL(string URL)
    {
        this.backgroundURL = URL;
    }

    public string getURL()
    {
        return this.backgroundURL;
    }

    public void setLastupdated(DateTime updated)
    {
        this.lastUpdated = updated;
    }

    public DateTime getLastUpdated()
    {
        return this.lastUpdated;
    }

    public void setBy(string by)
    {
        this.lastUpdatedBy = by;
    }

    public string getBy()
    {
        return this.lastUpdatedBy;
    }
}