using UnityEngine;

public static class PlayerData
{
    public static int MAX_ENERGY = 5;
    public static float ENERGY_TIME = 1800f;

    public static string ENERGY_TIME_KEY = "EnergyTime";
    // ===== COIN =====
    public static int GetCoin()
    {
        return PlayerPrefs.GetInt("Coin", 200);
    }
    public static void AddCoin(int amount)
    {
        int coin = GetCoin() + amount;
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.Save();
    }

    public static void SetCoin(int amount)
    {
        PlayerPrefs.SetInt("Coin", amount);
    }

    // ===== ENERGY =====
    public static int GetEnergy()
    {
        int energy = PlayerPrefs.GetInt("Energy", MAX_ENERGY);

        if (energy >= MAX_ENERGY) return energy;

        string timeStr = PlayerPrefs.GetString(ENERGY_TIME_KEY, "");

        if (string.IsNullOrEmpty(timeStr)) return energy;

        System.DateTime lastTime = System.DateTime.Parse(timeStr);
        double seconds = (System.DateTime.Now - lastTime).TotalSeconds;

        int recovered = (int)(seconds / ENERGY_TIME);

        if (recovered > 0)
        {
            energy = Mathf.Min(MAX_ENERGY, energy + recovered);

            PlayerPrefs.SetInt("Energy", energy);

            PlayerPrefs.SetString(ENERGY_TIME_KEY, System.DateTime.Now.ToString());
        }

        return energy;
    }

    public static void AddEnergy(int amount)
    {
        int energy = Mathf.Min(MAX_ENERGY, GetEnergy() + amount);

        PlayerPrefs.SetInt("Energy", energy);

        if (energy < MAX_ENERGY)
        {
            PlayerPrefs.SetString(ENERGY_TIME_KEY, System.DateTime.Now.ToString());
        }

        PlayerPrefs.Save();
    }

    public static void UseEnergy(int amount)
    {
        int energy = Mathf.Max(0, GetEnergy() - amount);

        PlayerPrefs.SetInt("Energy", energy);

        PlayerPrefs.SetString(ENERGY_TIME_KEY, System.DateTime.Now.ToString());

        PlayerPrefs.Save();
    }

    // ===== CHARACTER =====
    public static bool IsOwned(int id)
    {
        return PlayerPrefs.GetInt("Char_" + id, id == 0 ? 1 : 0) == 1;
    }

    public static void SetOwned(int id)
    {
        PlayerPrefs.SetInt("Char_" + id, 1);
    }

    public static int GetSelectedChar()
    {
        return PlayerPrefs.GetInt("SelectedChar", 0);
    }

    public static void SetSelectedChar(int id)
    {
        PlayerPrefs.SetInt("SelectedChar", id);
        PlayerPrefs.Save();
    }

    // ===== SHIELD =====
    public static int GetShield()
    {
        return PlayerPrefs.GetInt("Shield", 0);
    }

    public static void AddShield(int amount)
    {
        int shield = GetShield() + amount;
        PlayerPrefs.SetInt("Shield", shield);
        PlayerPrefs.Save();
    }

    public static void SetShield(int amount)
    {
        PlayerPrefs.SetInt("Shield", amount);
    }

    public static void UseShield(int amount)
    {
        int shield = Mathf.Max(0, GetShield() - amount);
        PlayerPrefs.SetInt("Shield", shield);
        PlayerPrefs.Save();
    }
}