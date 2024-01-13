using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BombController : MonoBehaviour
{


    [Header("Bomb")]
    public GameObject bombPrefabs;

    public KeyCode bombInputKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;

    [Header("Explosion")]
    public Explosion explosionPrefabs;
    public LayerMask explosionLayerMask;
    public LayerMask bombLayerMask;

    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTile;
    public Destructible destructible;

    private int bombsRemaining;


    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(bombInputKey))
        {
            StartCoroutine(IPlaceBomb());
        }
    }

    private IEnumerator IPlaceBomb()
    {
        // Rounded the bomb position
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefabs, (Vector3)position, quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        // bomb explode

        // The Explosion will happen on bottom of the bomb
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // The middle of the explosion
        Explosion explosion = Instantiate(explosionPrefabs, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);

        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        // Recursion to explode the bomb in 4 direction until reach the length
        if (length <= 0)
        {
            return;
        }

        position += direction;

        // If the bomb hit the block/brick then stop recursion + destroy the brick
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

         // If the bomb hit the other bomb then stop recursion
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, bombLayerMask))
        {
            return;
        }

        Explosion explosion = Instantiate(explosionPrefabs, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);

        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);

    }

    private void ClearDestructible(Vector2 position)
    {
        // Find the brick in tilemap by position 
        Vector3Int cell = destructibleTile.WorldToCell(position);
        TileBase tile = destructibleTile.GetTile(cell);

        // Instantiate the brick destroy animation then set tilemap to null
        if (tile != null)
        {
            Instantiate(destructible, position, Quaternion.identity);
            destructibleTile.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }


}
