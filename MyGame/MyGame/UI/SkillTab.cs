using Microsoft.Xna.Framework;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGame.UI
{
    class SkillTab
    {

        private List<Skill> skills;
        public bool IsVisible { get; set; }
        public Vector2 Position { get; set; }

        public SkillTab(Vector2 position)
        {
            this.Position = position;
            this.IsVisible = false;

            skills = new List<Skill>();
            float verticalOffset = 0; // Adjust this value to increase the space between skills
            skills.Add(new Skill(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 - 28 + verticalOffset), "Experience"));
            verticalOffset += 20; // Increase the offset for the next skill
            skills.Add(new Skill(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 - 28 + verticalOffset), "Level"));
            verticalOffset += 20; // Increase the offset for the next skill
            skills.Add(new Skill(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 - 28 + verticalOffset), "Sword"));
        }

        public void Draw()
        {
            if (IsVisible)
            {
                foreach (Skill skill in skills)
                {
                    skill.Draw();
                }
            }
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            this.Position = newPosition;

            // Initial vertical position
            float verticalPosition = newPosition.Y;

            // Vertical spacing between skills
            float verticalSpacing = 24;

            // Update the position of each skill
            foreach (var skill in skills)
            {
                skill.UpdatePosition(new Vector2(newPosition.X, verticalPosition));
                verticalPosition += verticalSpacing;
            }
        }
    }
}
