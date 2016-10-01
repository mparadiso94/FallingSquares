using UnityEngine;
using System.Collections;
using System;

// @NOTE the attached sprite's position should be "Top Right" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
[RequireComponent(typeof(SpriteRenderer))]

// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
public class RepeatSpriteBoundary : MonoBehaviour
{
    public float gridX = 0.0f;
    public float gridY = 0.0f;
    public int multiplier;

    SpriteRenderer sprite;

    void Awake()
    {

        sprite = GetComponent<SpriteRenderer>();

        Vector2 spriteSize_wu = new Vector2(sprite.bounds.size.x * multiplier / transform.localScale.x, sprite.bounds.size.y * multiplier / transform.localScale.y);
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

        if (0.0f != gridX)
        {
            float width_wu = sprite.bounds.size.x / gridX;
            scale.x = width_wu / spriteSize_wu.x;
            spriteSize_wu.x = width_wu;
        }

        if (0.0f != gridY)
        {
            float height_wu = sprite.bounds.size.y / gridY;
            scale.y = height_wu / spriteSize_wu.y;
            spriteSize_wu.y = height_wu;
        }

        GameObject childPrefab = new GameObject();

        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.position = transform.position;
        childSprite.sprite = sprite.sprite;
        childSprite.color = new Color32(255,255,255,75);

        GameObject child;
        for (int i = 0, h = (int)Mathf.Round(sprite.bounds.size.y); i * spriteSize_wu.y < h; i++)
        {
            for (int j = 0, w = (int)Mathf.Round(sprite.bounds.size.x); j * spriteSize_wu.x < w; j++)
            {
                child = Instantiate(childPrefab) as GameObject;
                child.transform.position = transform.position - (new Vector3(spriteSize_wu.x * j, spriteSize_wu.y * i, 0));
                child.transform.localScale = scale * multiplier;
                child.transform.parent = transform;
            }
        }

        Destroy(childPrefab);
        sprite.enabled = false; // Disable this SpriteRenderer and let the prefab children render themselves

        /*
		var x = SaveGameData.save.gameData.patternIndex;
		foreach (var y in transform.GetComponentsInChildren<SpriteRenderer>())
			y.sprite = ChangeTheme.chngThm.themesDatabase.GetPatternAt(x).sprite;
            */
    }
	/*
    public void ChangeSprite(int x)
    {
		if (SaveGameData.save.gameData.unlockedPatterns [x] == 0 && SaveGameData.save.gameData.grabs >= SaveGameData.save.gameData.patternCost) {
			SaveGameData.save.gameData.patternCount++;
			SaveGameData.save.gameData.unlockedPatterns [x] = 1;
			SaveGameData.save.gameData.patternIndex = x;
			//if (SaveGameData.save.gameData.trueCost){
				SaveGameData.save.gameData.grabs -= SaveGameData.save.gameData.patternCost;
			//} else {
			//	SaveGameData.save.gameData.patternCost += 100;
			//}
			SaveGameData.save.gameData.patternCost += 100;
			SaveGameData.save.Save ();
			foreach (var y in transform.GetComponentsInChildren<SpriteRenderer>()) {
				y.sortingOrder = -10;
				y.sprite = ChangeTheme.chngThm.patTiles [x].pTile;
			}
		} else if (SaveGameData.save.gameData.unlockedPatterns [x] == 1) {
			SaveGameData.save.gameData.patternIndex = x;
			SaveGameData.save.Save ();
			foreach (var y in transform.GetComponentsInChildren<SpriteRenderer>()) {
				y.sortingOrder = -10;
				y.sprite = ChangeTheme.chngThm.patTiles [x].pTile;
			}
		} else {
			Debug.Log("Too Expensive");
		}
    }*/

	public void ChangeSprite(Sprite sprite)
	{
		foreach (var y in transform.GetComponentsInChildren<SpriteRenderer>()) {
			y.sortingOrder = -10;
			y.sprite = sprite;
		}
	}
}