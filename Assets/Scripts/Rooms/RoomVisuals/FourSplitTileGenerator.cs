#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;


public class FourSplitTileGenerator : EditorWindow
{
    [MenuItem("Custom Creator/Four Split Tile Generator")]
    public static void ShowWindow()
    {
        GetWindow<FourSplitTileGenerator>("4 Split Tiles Generator");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Split Texture", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        

        _cellSize = EditorGUILayout.IntField("Cell Size", _cellSize);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Border");
        _spriteSheet = (Texture2D)EditorGUILayout.ObjectField(_spriteSheet, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Mask");
        _mask = (Texture2D)EditorGUILayout.ObjectField(_mask, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Repeating");
        _repeatingTexture = (Texture2D)EditorGUILayout.ObjectField(_repeatingTexture, typeof(Texture2D), false);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Split Texture!"))
        {
            SplitSprite();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Create Rule Tile", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        tileName = EditorGUILayout.TextField("Tile name", tileName);
        EditorGUILayout.Space();

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty prp = so.FindProperty("_sprites");
        EditorGUILayout.PropertyField(prp, true);
                

        if (GUILayout.Button("Create tile!"))
        {
            GenerateRuleTile();
        }
        

        so.ApplyModifiedProperties();
    }

    public Texture2D _spriteSheet;
    public Texture2D _mask;
    public Texture2D _repeatingTexture;
    public int _cellSize;

    public Sprite[] _sprites;
    public RuleTile _template;

    public string tileName;
    
    TextureImporter _textureImporter;

    void SplitSprite()
    {     
        string path = UnityEditor.AssetDatabase.GetAssetPath(_spriteSheet);

        _textureImporter = (TextureImporter)TextureImporter.GetAtPath(path);
        _textureImporter.textureType = TextureImporterType.Sprite;
        _textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        _textureImporter.mipmapEnabled = false;
        _textureImporter.filterMode = FilterMode.Point;
        _textureImporter.spritePixelsPerUnit = _cellSize - 2;


        var factory = new SpriteDataProviderFactories();
        factory.Init();
        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(_textureImporter);
        dataProvider.InitSpriteEditorDataProvider();

        SpriteRect[] createdRects = new SpriteRect[8];
        int i = 0;
        for (int r = 0; r <= 3; ++r)
        {
            for (int c = 0; c <= 1; ++c)
            {
                SpriteRect rect = new SpriteRect();
                rect.rect = new Rect((c * _cellSize) + 1, (r * _cellSize) + 1, _cellSize - 2, _cellSize - 2);
                rect.name = c + "-" + r;
                createdRects[i] = rect;
                i++;
            }
        }
        dataProvider.SetSpriteRects(createdRects);
        dataProvider.Apply();
              
        SecondarySpriteTexture mask = new SecondarySpriteTexture();
        mask.texture = _mask;
        mask.name = "_Mask";

        SecondarySpriteTexture repeat = new SecondarySpriteTexture();
        repeat.texture = _repeatingTexture;
        repeat.name = "_RepeatingTexture";
               
        _textureImporter.secondarySpriteTextures = new SecondarySpriteTexture[] { repeat, mask };

        EditorUtility.SetDirty(_textureImporter);
        _textureImporter.SaveAndReimport();
    }

    public List<List<int>> NeighborPositions = new List<List<int>>()
    {
        new List<int>() { 0, 1, 2, 1, 1, 0, 1, 0},
        new List<int>() { 0, 1, 0, 1, 2, 0, 1, 0},
        new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1},
        new List<int>() { 0, 1, 0, 1, 1, 0, 1, 2},
        new List<int>() { 0, 2, 0, 1, 1, 0, 1, 0},
        new List<int>() { 0, 1, 0, 2, 1, 0, 2, 0},
        new List<int>() { 0, 2, 0, 2, 1, 0, 1, 0},
        new List<int>() { 0, 1, 0, 1, 1, 0, 2, 0},
    };

    public void GenerateRuleTile()
    {
        RuleTile tile = ScriptableObject.CreateInstance<RuleTile>();
        
        for (int i = 0; i < 8; i++)
        {

            RuleTile.TilingRule rule = new RuleTile.TilingRule();
            rule.m_Sprites[0] = _sprites[i];
            rule.m_Neighbors = NeighborPositions[i];
            rule.m_RuleTransform = RuleTile.TilingRuleOutput.Transform.MirrorX;

            tile.m_TilingRules.Add(rule);
        }

        UnityEditor.AssetDatabase.CreateAsset(tile, $"Assets/Tiles/RuleTiles/{tileName}.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = tile;

    }


}
#endif