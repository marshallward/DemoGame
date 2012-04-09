using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TiledSharp;
using System.IO;

namespace JRPG
{    
    public class Zone
    {
        // Terrain information
        public List<uint[,]> map;              // Map layers
        public List<Texture2D> tileset;        // Tilesets
        public int layer_width, layer_height;  // Layer dimensions
        public int tile_width, tile_height;    // Pixel shape of tiles

        public float tile_scale;

        public List<Tile> map_tiles;

        // Navigation
        public Vector2 position;
        public Vector2 last_position;
        public Vector2 direction;
        public Vector2 last_direction;
        public float walking_speed = 30.0f; // Use 60Hz ratios
        public bool is_walking;

        public static Dictionary<ButtonState, Vector2> pad_dir;
        
        // Incorporate a Tiled object's contents into a Zone object
        public static Zone Load(Map map, ContentManager content)
        {
            var zone = new Zone();
            
            // Get dimensions
            zone.tile_width = map.tilewidth;
            zone.tile_height = map.tileheight;

            // Copy the layer tile maps
            zone.map = new List<uint[,]>();
            foreach (var layer in map.layer)
                zone.map.Add(layer.data);

            // Process the tile images
            zone.tileset = new List<Texture2D>();
            foreach (var ts in map.tileset)
            {
                var tileset_path = Path.GetFileNameWithoutExtension(ts.image.source);
                zone.tileset.Add(content.Load<Texture2D>(tileset_path));
            }
            
            // Process individual tiles as a list
            zone.map_tiles = new List<Tile>();


            // Initial position
            zone.position = Vector2.Zero;
            zone.tile_scale = 3.0f;

            return zone;
        }
        
        public void Draw(SpriteBatch batch)
        {
            // Kind of a mess... need to consolidate here!
            
            // Draw the tile map (slow?)
            foreach(var m in map)
                for (int j = 0; j < m.GetLength(1); j++)
                    for (int i = 0; i < m.GetLength(0); i++)
                        if (m[i,j] != 0) // Need a more elegant mask control
                        {
                            var tile_index = m[i, j] - 1;
                            var pos = new Vector2((float)(tile_width * tile_scale * i),
                                                  (float)(tile_height * tile_scale * j));
                            // Need to reduce rect to the game window
                            var rect = new Rectangle(tile_width * (int)tile_index, 0, tile_width, tile_height);
                            batch.Draw(tileset[0], pos, rect, Color.White, 
                                 0.0f, this.position, tile_scale, SpriteEffects.None, 0);
                        }
        } // end Draw

        public void DrawTiles(Zone zone)
        {
            // Test method to draw a list of tiles
        }

        public void UpdatePosition(GamePadState pad_state, GameTime game_time)
        {
            // Note: We must update before checking, and cannot use an 'else'
            // (Not 100% on the logic here; consider refactoring)

            if (is_walking)
            {
                // Increment the position
                position += direction * walking_speed * (float)game_time.ElapsedGameTime.TotalSeconds;

                var x = (int)(position.X - last_position.X);
                var y = (int)(position.Y - last_position.Y);
                // Flip to not walking if displacement has reached a tile width/height
                if (Math.Abs(x) >= tile_width || Math.Abs(y) >= tile_height)
                {
                    is_walking = false;
                    last_direction = direction;
                }
            }

            // Check if gamepad is pressed, reinstate if necessary
            if (!is_walking)
            {
                direction = new Vector2(0, 0);

                if (pad_state.DPad.Left == ButtonState.Pressed)
                    direction -= Vector2.UnitX;
                if (pad_state.DPad.Right == ButtonState.Pressed)
                    direction += Vector2.UnitX;
                if (pad_state.DPad.Up == ButtonState.Pressed)
                    direction -= Vector2.UnitY;
                if (pad_state.DPad.Down == ButtonState.Pressed)
                    direction += Vector2.UnitY;

                if (direction != Vector2.Zero)
                {
                    last_position = position;
                    is_walking = true;
                }
            }

            //Console.WriteLine(is_walking.ToString());
            //Console.WriteLine(position.ToString());
        }
    } // end Zone
    
    public class Tile
    {
        // Abstraction
        public int x, y;        // Tile coordinate (x,y)
        public bool visible;
        // + properties(wall, water, etc)

        // Rendering
        public Texture2D set;           // Source tileset
        public Rectangle rect;   // Tileset bounding box
        public Rectangle window_rect;   // Window bounding box

        public Tile(int x_in, int y_in, Texture2D tileset, Rectangle set_rect)
        {
            set = tileset;
            rect = set_rect;
            x = x_in;
            y = y_in;
        }
    }
}
