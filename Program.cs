/*
    * An AI Coded by pradosh-arduino
      => https://github.com/pradosh-arduino/XeonAI
    ! This AI is made of Neural Network AND requires **Heavy** Computing Power
    ! Make sure you have a good PC/Laptop
    ! If not, use this at your own **RISK**
    ? This program is indexed by 1 - Because 0 neurons doesn't make sense
*/

namespace pradosh_arduino
{
    class XeonAI
    {
        static int InputNeuronCount = 2+1, OutputNeuronCount = 2+1, ProcessNeuronCount = 3+1; // We are using +1 because indexing by 1
        static int iteration = 10000, generation = 0, TotalSuccessiveGen = 0, TotalGeneration = 0; // don't play with TotalSuccessiveGen or generation
        static float[] inputvalues = new float[InputNeuronCount];
        static float[] outputvalues = new float[OutputNeuronCount];
        static int objective = 30; // AI's Objective 
        static int currentRandom1 = 0, currentRandom2 = 0;
        static int LearningRate = 1; // 1 - 10 is best, Lower the value more accurate it is
        /* 
           ^ This is like when you show a person a cat and a dog then he is like "hmm... Can I see more?" - This part means Learning Rate is Low but He Learns and can differentiate
           ^ Now you Increase the learning rate - Now you show a picture of a fully cat and he tells it is a dog 🤦‍♀️ - See? He learnt quickly but wrong
        */
        static float Momentum = 0.464634834f; // 0.0 - 1.0 is best; More decimal length more accurate it is
        static bool infiniteLearning = true; // If you enable this the AI learns more and more rather than terminating
        static int[] lastGen = new int[iteration]; // this is used for getting the last successfull generation
        static int ImprovementCount = 0, NonImprovementCount = 0;
        public static void Main(string[] args){
            XeonAI xeonAI = new XeonAI();
            Console.Clear();
            xeonAI.AIMain();
        }
        static bool isBrianLoaded = false;
        // We are escaping out of the static world
        public void AIMain(){
            Neural_Network nn = new Neural_Network();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Welcome to XeonAI This is An AI Made by ⚡ pradosh-arduino with Neural Network 🧠");
            Console.WriteLine("Make sure you have a high computing machine - If you want you can Monitor your Machine - https://github.com/pradosh-arduino/XeonAI/ \nAI will start in 10 seconds.");
            Thread.Sleep(10000);
            Console.WriteLine("Started!");
            if(File.Exists(".xaiBrain")){
                int fail = nn.LoadBrain();
                if(fail == 1){isBrianLoaded = false;}else{isBrianLoaded = true;}
            }
            Random random = new Random();
            int[] currentValue = new int[InputNeuronCount];
            for(int i=0; i<iteration; i++, generation++, TotalGeneration++){
                if(isBrianLoaded == false){
                    currentValue[1] = random.Next(0, 999);
                    currentValue[2] = random.Next(0, 999);
                    currentRandom1 = currentValue[1];
                    currentRandom2 = currentValue[2];
                }else{
                    currentValue[1] = currentRandom1;
                    currentValue[2] = currentRandom2;
                }
                nn.Input(currentValue);
                nn.Process();
                nn.Output();
                if(Convert.ToInt32(outputvalues[1]) == objective || outputvalues[2] == objective){
                    Console.WriteLine("FOUND Objective - Generation Took: {0} Sucessive Generation: {1} Current Generation: {2}", generation, TotalSuccessiveGen, TotalGeneration);
                    TotalSuccessiveGen++;
                    if(infiniteLearning == false){
                        Console.WriteLine("Do you want to save the brain? [Y/n]");
                        againInput:
                        string yrn = Console.ReadLine()!;
                        if(yrn == "Y" || yrn == "y"){
                            nn.SaveBrain();
                            return;
                        }else if(yrn == "N" || yrn == "n"){
                            return;
                        }else{
                            Console.WriteLine("Please enter y/n");
                            goto againInput;
                        }
                    }else{
                        lastGen[TotalSuccessiveGen] = generation;
                        if(lastGen[TotalSuccessiveGen-1] > generation){
                            ImprovementCount++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Improvement Found.");
                            Console.ResetColor();
                            if(generation <= 10){
                                Console.WriteLine("This seems to be very successfull. Do you want to save the brain? [Y/n]");
                                // & If you want you can disable this beep I have added this because I am going to run this AI for straight 24 Hours and see what have it learnt
                                Console.Beep(700, 1000);

                                againInput:
                                string yrn = Console.ReadLine()!;
                                if(yrn == "Y" || yrn == "y"){
                                    nn.SaveBrain();
                                }else if(yrn == "N" || yrn == "n"){

                                }else{
                                    Console.WriteLine("Please enter y/n");
                                    goto againInput;
                                }
                            }
                        }else if(lastGen[TotalSuccessiveGen-1] < generation){
                            NonImprovementCount++;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No Improvement Found.");
                            Console.ResetColor();
                            nn.SelfModify();
                        }
                        generation = 0;
                        Thread.Sleep(200);
                    }
                } else {
                    nn.SelfModify();
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Number of times that found Improvement: " + ImprovementCount);
            Console.WriteLine("Number of times that found no improvement: " + NonImprovementCount);
            Console.ResetColor();
        }

        class Neural_Network 
        {
            float[] weight_1 = new float[InputNeuronCount];
            float[] weight_2 = new float[OutputNeuronCount];
            float[] bias = new float[ProcessNeuronCount];
            double[] processValue = new double[ProcessNeuronCount];
            public void Input(int[] value){
                int oddeven = 0;

                if(isBrianLoaded == false){weight_1[1] = 0.2324f; weight_1[2] = 0.3482f;} // Any values will work
                else {} // Do nothing. Which means the Brain as loaded with values
                if(value[1] % 2 == 0){oddeven = 2;}
                else {oddeven = 2;}
                inputvalues[1] = weight_1[1] * value[1] + oddeven;
                inputvalues[2] = weight_1[2] * value[2] + oddeven;
            }

            public void SaveBrain(){
                File.WriteAllText(".xaiBrain","XeonAI\n");
                File.AppendAllText(".xaiBrain", "1:"+currentRandom1+"\n");
                File.AppendAllText(".xaiBrain", "2:"+currentRandom2+"\n");
                for (int i = 1; i < InputNeuronCount; i++)
                {
                    File.AppendAllText(".xaiBrain",$"n1({i}):"+weight_1[i]+"\n"); 
                }
                for (int i = 1; i < OutputNeuronCount; i++)
                {
                    File.AppendAllText(".xaiBrain",$"n2({i}):"+weight_2[i]+"\n"); 
                }
                for (int i = 1; i < ProcessNeuronCount; i++)
                {
                   File.AppendAllText(".xaiBrain",$"b1({i}):"+bias[i]+"\n"); 
                }
            }
            private string GetLine(int line)
		    {
		       using (var sr = new StreamReader(".xaiBrain")) {
		    	   for (int i = 1; i < line; i++)
		    		  sr.ReadLine();
		    		return sr.ReadLine()!; 
		       }
		    }
            public int LoadBrain(){
                string sign = GetLine(1);
                if(sign != "XeonAI"){
                    Console.WriteLine("Signature is Invalid!");
                    return 1;
                }
                currentRandom1 = Convert.ToInt32(GetLine(2).Remove(0, 2));
                currentRandom2 = Convert.ToInt32(GetLine(3).Remove(0, 2));
                weight_1[1] = (float)Convert.ToDouble(GetLine(4).Remove(0, 6));
                weight_1[2] = (float)Convert.ToDouble(GetLine(5).Remove(0, 6));
                weight_2[1] = (float)Convert.ToDouble(GetLine(6).Remove(0, 6));
                weight_2[2] = (float)Convert.ToDouble(GetLine(7).Remove(0, 6));
                bias[1] = (float)Convert.ToDouble(GetLine(8).Remove(0, 6));
                bias[2] = (float)Convert.ToDouble(GetLine(9).Remove(0, 6));
                bias[3] = (float)Convert.ToDouble(GetLine(10).Remove(0, 6));
                return 0;
            }

            public void Process(){
                if(isBrianLoaded == false){bias[1] = 0.78f; bias[2] = 0.394f; bias[3] = 0.982032f;} // Add more `bias` to increase the Process Neuron (this also can be any values)
                else {}
                processValue[1] = (inputvalues[1] + weight_1[1] * inputvalues[2] + weight_1[2]) + bias[1];
                processValue[2] = (inputvalues[1] + weight_1[1] * inputvalues[2] + weight_1[2]) + bias[2];
                processValue[3] = (inputvalues[1] + weight_1[1] * inputvalues[2] + weight_1[2]) + bias[3];
            }

            public void Output(){
                int oddeven = 0;
                if(isBrianLoaded == false){weight_2[1] = 0.2324f; weight_2[2] = 0.3482f;} // Any values will work
                else {}
                if(processValue[1] % 2 == 0){oddeven = 2;}
                else {oddeven = 2;}
                outputvalues[1] = weight_1[1] * (float)processValue[1] + oddeven;
                outputvalues[2] = weight_1[2] * (float)processValue[2] + oddeven;
                outputvalues[1] += weight_1[2] * (float)processValue[1] + oddeven;
                outputvalues[2] += weight_1[1] * (float)processValue[2] + oddeven;
            }

            public void SelfModify(){

                // ~ formula: change = (learningRate * delta * value) + (momentum * pastChange)
                float delta = 1/2*((objective - weight_1[1]+weight_1[2])*(objective - weight_1[1]+weight_1[2]));
                float[] prevBias = new float[ProcessNeuronCount]; prevBias = bias;

                bias[1] = (LearningRate * delta * 0.231987342648346748f) + (Momentum * prevBias[1]);
                bias[2] = (LearningRate * delta * 0.231987342648346748f) + (Momentum * prevBias[2]);
                bias[3] = (LearningRate * delta * 0.231987342648346748f) + (Momentum * prevBias[3]);
            }
        }
    }
}