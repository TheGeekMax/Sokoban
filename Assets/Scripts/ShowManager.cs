using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//pour les tilemaps
using UnityEngine.Tilemaps;

public class ShowManager : MonoBehaviour
{
    public static ShowManager instance;

    public Tilemap tilemap;
    public TileBase baseTile;
    public TileBase wallTile;
    public TileBase boxTile;
    public TileBase playerTile;
    public TileBase winTile;
    public TileBase winTileBox;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    int[,] plateau;
    
    public void Setup(){
        plateau = GameManager.instance.GetPlateau();
        //on affiche le plateau
        for(int i = 0; i < GameManager.instance.GetWidth(); i++){
            for(int j = 0; j < GameManager.instance.GetHeight(); j++){
                if(plateau[i, j] == 0){
                    tilemap.SetTile(new Vector3Int(i, j, 0), baseTile);
                }
                else if(plateau[i, j] == 1){
                    tilemap.SetTile(new Vector3Int(i, j, 0), wallTile);
                }
                else if(plateau[i, j] == 2){
                    tilemap.SetTile(new Vector3Int(i, j, 0), boxTile);
                }
                else if(plateau[i, j] == 3){
                    tilemap.SetTile(new Vector3Int(i, j, 0), playerTile);
                }
            }
        }
        //on met le tile de win si il y a rien dessu
        Vector2Int[] goalPos = GameManager.instance.GetGoalPos();
        for(int i = 0; i < goalPos.Length; i++){
            if(plateau[goalPos[i].x, goalPos[i].y] == 0){
                tilemap.SetTile(new Vector3Int(goalPos[i].x, goalPos[i].y, 0), winTile);
            }
            else if(plateau[goalPos[i].x, goalPos[i].y] == 2){
                tilemap.SetTile(new Vector3Int(goalPos[i].x, goalPos[i].y, 0), winTileBox);
            }
        }
    }

    public void UpdateTilemap(){
        int[,] newP = GameManager.instance.GetPlateau();
        for(int i = 0; i < GameManager.instance.GetWidth(); i++){
            for(int j = 0; j < GameManager.instance.GetHeight(); j++){
                if(plateau[i, j] != newP[i, j]){
                    if(newP[i, j] == 0){
                        tilemap.SetTile(new Vector3Int(i, j, 0), baseTile);
                    }
                    else if(newP[i, j] == 1){
                        tilemap.SetTile(new Vector3Int(i, j, 0), wallTile);
                    }
                    else if(newP[i, j] == 2){
                        tilemap.SetTile(new Vector3Int(i, j, 0), boxTile);
                    }
                    else if(newP[i, j] == 3){
                        tilemap.SetTile(new Vector3Int(i, j, 0), playerTile);
                    }
                }
            }
        }
        plateau = newP;
        //on met le tile de win si il y a rien dessu
        Vector2Int[] goalPos = GameManager.instance.GetGoalPos();
        for(int i = 0; i < goalPos.Length; i++){
            if(plateau[goalPos[i].x, goalPos[i].y] == 0){
                tilemap.SetTile(new Vector3Int(goalPos[i].x, goalPos[i].y, 0), winTile);
            }
            else if(plateau[goalPos[i].x, goalPos[i].y] == 2){
                tilemap.SetTile(new Vector3Int(goalPos[i].x, goalPos[i].y, 0), winTileBox);
            }
        }
    }
}
