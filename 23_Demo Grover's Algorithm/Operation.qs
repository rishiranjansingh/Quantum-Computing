﻿namespace Quantum._23_Demo_Grover_s_Algorithm
{
    open Microsoft.Quantum.Primitive;
    open Microsoft.Quantum.Canon;
	open Microsoft.Quantum.Extensions.Math;
	open Microsoft.Quantum.Extensions.Convert;

	operation GetNumberOfIterations (n: Double): (Double)
	{
		body
		{
			mutable root = 1.0;
			set root = 1.0/Sqrt(n);
			mutable angle = 1.0;
			mutable pi = PI();
			set angle = (PI()/(4.0* ArcSin(root))) - 0.5;
			return angle;
		}
	}

	//The blackbox is implemented here.
	//This is a generic black box that flips the extraBit only if the qubits match the controlString
	operation GenericFlip(qubits: Qubit[], controlString: Bool[], extraBit: Qubit, n: Int): ()
	{
		body
		{
			//Changing the qubits to 1 whereever we have 0
			for(i in 0..n-1)
			{
				if(controlString[i] == false)
				{
					X(qubits[i]);
				}
			}

			//Flipping the extraBit; Now, X will be applieed only if bits match the controlString
			(Controlled X)(qubits, extraBit);

			//Undoing the changes we made in the first for loop
			for(i in 0..n-1)
			{
				if(controlString[i] == false)
				{
					X(qubits[i]);
				}
			}
		}
	}

	//This will reflect the amplitudes about the meain.
	//A video will be uploaded to explain this logic
	operation GenericReflectionAboutMean(qubits: Qubit[], extraBit: Qubit): ()
	{
		body
		{
			//Change of bases so that |00000> aligns with H*H*H*H*H(|00000>)
			ApplyToEach(H, qubits);

			//Flip the extraBit only for the states != |00000>
			ApplyToEach(X, qubits);//This will make |00000> to |11111>
			X(extraBit);//This will always flip the extraBit
			(Controlled X)(qubits, extraBit);//This will flip only if the state is |11111>, this step and above step will effectively
			//ensure that except for |00000>, all other states's amplitudes will be negated
			ApplyToEach(X, qubits);//This will undo the above ApplyToEach

			//Undoing the basis change
			ApplyToEach(H, qubits);
		}
	}

	//n is the numer of qubits we are using
	//pattern is the number we are trying to search
	operation GenericGroversAlgo (n: Int, pattern: Bool[]) : (Int)
    {
        body
        {
			using(qubits = Qubit[n])
			{
				using(extraBit = Qubit[1])
				{
					ApplyToEach(H, qubits);

					X(extraBit[0]);
					H(extraBit[0]);

					mutable iter = 1;
					set iter = Round(GetNumberOfIterations(PowD(ToDouble(n),2.0)));

					for(i in 1..iter)
					{
						GenericFlip(qubits, pattern, extraBit[0], n);
						GenericReflectionAboutMean(qubits, extraBit[0]);
					}
					
					//Measuing the system and generating a binary string
					mutable str = "";
					let results = MultiM(qubits);
					for(i in 0..Length(qubits)-1)
					{
						set str = str + ToStringI(ResultAsInt([results[i]]));
					}
					Message("State of the system at the end of " + ToStringI(iter) + " iterations:");
					Message(str);

					
					ResetAll(qubits);
					ResetAll(extraBit);
				}	
			}

			//Returning some dummy result
			return 1;
            
        }
    }
}
