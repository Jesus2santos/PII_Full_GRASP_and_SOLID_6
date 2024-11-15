namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent
    {
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }
        public bool Cooked { get; private set; } = false;  // Cooked empieza en false

        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        // Nuevo método: Sumar el tiempo de todos los pasos
        public int GetCookTime()
        {
            int totalTime = 0;
            foreach (BaseStep step in this.steps)
            {
                totalTime += step.Time;  // Asumimos que `Time` está definido en la clase `BaseStep`
            }
            return totalTime;
        }

        // Nuevo método: Comienza la cocción utilizando el temporizador
        public void Cook()
        {
            if (!Cooked)  // La receta solo se puede cocinar una vez
            {
                CountdownTimer timer = new CountdownTimer();
                timer.Register(GetCookTime(), new RecipeCooker(this)); // Usamos un cliente Timer
                Cooked = false; // Se pondrá a true cuando el temporizador termine
            }
        }
    }

    // Implementación del TimerClient para Recipe
    public class RecipeCooker : TimerClient
    {
        private Recipe recipe;

        public RecipeCooker(Recipe recipe)
        {
            this.recipe = recipe;
        }

        public void TimeOut()
        {
            recipe.Cooked = true;  // Al terminar el temporizador, cambiamos Cooked a true
        }
    }
}
