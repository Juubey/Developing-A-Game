using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;

    public ColorToPrefab[] colorMappings;

    void Start()
    {
        GenerateLevel();    
    }

void GenerateLevel()
{
    for (int x = 0; x < map.width; x++)
    {
         for (int y = 0; y < map.height; y++)
        {
            GenerateTile(x,y);
        }
        
    }

}

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x,y);

        if (pixelColor.a == 0)
        {
            // The pixel is transparent. Lets ignore it!
            return;
        }

        foreach (ColorToPrefab colorMappings in colorMappings)
        {
            if (colorMappings.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x,y);
                Instantiate(colorMappings.prefab, position, Quaternion.identity, transform);
            }
        }
    }

}
