// <copyright file="Perceptron_PCZ.qs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Quantum.PQC
{
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Canon;

	/// <summary>
	/// This operation is designed for dense region classification from provided vector using dotproduct calculation
	/// </summary>
	operation Perceptron_PCZ (inputVector : Int [][], weightVector: Int[][], qubitCount: Int) : Int {
	using(q = Qubit[qubitCount+1])
		{
			mutable countOne = 0;

			for(i in 1..8192)
			{
				ApplyToEach(H, q[0..qubitCount-1]);

				//mutable q = Subarray([0,1,3], qbits);

				ApplyZstoVector(q[0..qubitCount-1], inputVector, qubitCount);
				ApplyZstoVector(q[0..qubitCount -1], weightVector, qubitCount);
				ApplyToEach(H, q[0..qubitCount -1]);
				ApplyToEach(X, q[0..qubitCount -1]);
				Controlled X(q[0..qubitCount - 1], q[qubitCount]);

				let r = M(q[qubitCount]);

				if(r == One)
				{
					set countOne = countOne + 1;
				}

				ResetAll(q);
			}

			return countOne;
		}
	}

	/// <summary>
	/// Method to apply Z and Controlled Z gate on the provided qubits based on the
	/// vector supplied
	/// </summary>
	operation ApplyZstoVector(qbits: Qubit[], vector: Int [][], qbitSize : Int) : Unit
	{
		mutable n = Length(vector);
		if(n != 0){
		for(i in 0..n-1){
			mutable vector1 = vector[i];
			mutable len = Length(vector1);
			if(len == 1){
				Z(qbits[vector1[0]]);
			}
			elif(len == 2){
				Controlled Z([qbits[vector1[0]]], qbits[vector1[1]]);
			}
			elif(len == 3){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]]], qbits[vector1[2]]);
			}elif(len == 4){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]], qbits[vector1[2]]], qbits[vector1[3]]);
			}elif(len == 5){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]], qbits[vector1[2]], qbits[vector1[3]]], qbits[vector1[4]]);
			}elif(len == 6){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]], qbits[vector1[2]], qbits[vector1[3]], qbits[vector1[4]]], qbits[vector1[5]]);
			}elif(len == 7){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]], qbits[vector1[2]], qbits[vector1[3]], qbits[vector1[4]], qbits[vector1[5]]], qbits[vector1[6]]);
			}elif(len == 8){
				Controlled Z([qbits[vector1[0]], qbits[vector1[1]], qbits[vector1[2]], qbits[vector1[3]], qbits[vector1[4]], qbits[vector1[5]], qbits[vector1[6]]], qbits[vector1[7]]);
			}
			else {
				Message("8+ Qubit count not supported..!!");
			}
		}}
	}
}