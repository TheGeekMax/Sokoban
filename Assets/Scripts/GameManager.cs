using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int width;
    int height;

    int[,] plateau;
    Vector2Int[] goalPos;

    //array list de la grille pour pouvoir undo
    ArrayList grille = new ArrayList();

    public static GameManager instance;

    
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    //ids : -1 = no wall, 0 = empty, 1=wall, 2=box, 3=player

    void Start(){
        //plateau example
        width = 9;
        height = 10;
        plateau = new int[9, 10] {
            {1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,1,1,0,0,1},
            {1,0,2,3,2,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,1},
            {1,1,1,1,1,1,1,0,1,1},
            {-1,-1,-1,-1,-1,-1,1,0,1,-1},
            {-1,-1,-1,-1,-1,-1,1,0,1,-1},
            {-1,-1,-1,-1,-1,-1,1,0,1,-1},
            {-1,-1,-1,-1,-1,-1,1,1,1,-1}
        };

        //on centre la camera
        Camera.main.transform.position = new Vector3(width / 2, height / 2, -10);

        //goalPos example
        goalPos = new Vector2Int[2];
        goalPos[0] = new Vector2Int(7, 7);
        goalPos[1] = new Vector2Int(6, 7);
        //a partir d'ici on execute les autres managers
        ShowManager.instance.Setup();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            //on ajoute la grille actuelle à l'historique
            SavePlateau();
            up();
            ShowManager.instance.UpdateTilemap();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            SavePlateau();
            right();
            ShowManager.instance.UpdateTilemap();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            SavePlateau();
            down();
            ShowManager.instance.UpdateTilemap();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            SavePlateau();
            left();
            ShowManager.instance.UpdateTilemap();
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            Undo();
            ShowManager.instance.UpdateTilemap();
        }
    }

    void SavePlateau(){
        //on ajoute la grille actuelle à l'historique en creant une copie
        int[,] copie = new int[width, height];
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                copie[i, j] = plateau[i, j];
            }
        }
        grille.Add(copie);
    }

    void Undo(){
        if(grille.Count == 0){
            return;
        }
        //on recupere la grille precedente
        int[,] copie = (int[,])grille[grille.Count - 1];
        //on l'applique
        plateau = copie;
        //on supprime la grille precedente
        grille.RemoveAt(grille.Count - 1);
    }

    public bool move(int x, int y, int or){
        // or -> 0 = up, 1 = right, 2 = down, 3 = left

        //etape 0 : test si OOB
        if(x < 0 || x >= width || y < 0 || y >= height){
            return false;
        }

        // etape 1 : test si vide
        if(plateau[x, y] == 0){
            return true;
        }

        // etape 2 : test si mur
        if(plateau[x, y] == 1 || plateau[x, y] == 3 || plateau[x, y] == -1){
            return false;
        }

        // etape 3 : test si caisse
        switch(or){
            case 0:
                bool free = move(x, y + 1, or);
                if(free){
                    plateau[x, y + 1] = 2;
                    plateau[x, y] = 0;
                    return true;
                }
                return false;
            case 1:
                free = move(x + 1, y, or);
                if(free){
                    plateau[x + 1, y] = 2;
                    plateau[x, y] = 0;
                    return true;
                }
                return false;
            case 2:
                free = move(x, y - 1, or);
                if(free){
                    plateau[x, y - 1] = 2;
                    plateau[x, y] = 0;
                    return true;
                }
                return false;
            default:
                free = move(x - 1, y, or);
                if(free){
                    plateau[x - 1, y] = 2;
                    plateau[x, y] = 0;
                    return true;
                }
                return false;
        }
    }

    //fonction de mouvements

    public void up(){
        //probleme suplementaire, ajout de l'historique des mouvements
        bool[,] historique = new bool[width, height];
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                historique[i, j] = true;
            }
        }
        //on regarde tout le tableau, dès qu'on vois le joueur on le déplace
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(plateau[i, j] == 3){
                    if(historique[i,j]){
                        bool free = move(i, j + 1, 0);
                        if(free){
                            historique[i, j + 1] = false;
                            plateau[i, j + 1] = 3;
                            plateau[i, j] = 0;
                        }
                    }
                }
            }
        }
    }

    public void right(){
        //probleme suplementaire, ajout de l'historique des mouvements
        bool[,] historique = new bool[width, height];
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                historique[i, j] = true;
            }
        }
        //on regarde tout le tableau, dès qu'on vois le joueur on le déplace
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(plateau[i, j] == 3){
                    if(historique[i,j]){
                        bool free = move(i + 1, j, 1);
                        if(free){
                            historique[i + 1, j] = false;
                            plateau[i + 1, j] = 3;
                            plateau[i, j] = 0;
                        }
                    }
                }
            }
        }
    }

    public void down(){
        //on regarde tout le tableau, dès qu'on vois le joueur on le déplace
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(plateau[i, j] == 3){
                    bool free = move(i, j - 1, 2);
                    if(free){
                        plateau[i, j - 1] = 3;
                        plateau[i, j] = 0;
                    }
                }
            }
        }
    }

    public void left(){
        //on regarde tout le tableau, dès qu'on vois le joueur on le déplace
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                if(plateau[i, j] == 3){
                    bool free = move(i - 1, j, 3);
                    if(free){
                        plateau[i - 1, j] = 3;
                        plateau[i, j] = 0;
                    }
                }
            }
        }
    }

    public int[,] GetPlateau(){
        int[,] copie = new int[width, height];
        for(int i = 0; i < width; i++){
            for(int j = 0; j < height; j++){
                copie[i, j] = plateau[i, j];
            }
        }
        return copie;
    }

    public Vector2Int[] GetGoalPos(){
        return goalPos;
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }
}
