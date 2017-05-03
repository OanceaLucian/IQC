// Copyright (c) 2015,2016 Microsoft Corporation

#if INTERACTIVE
#r @"/home/lucian/Liquid-master/bin/Liquid1.dll" // put your path here
#else
namespace Microsoft.Research.Liquid // Tell the compiler our namespace
#endif

open System                         // Open any support libraries

open Microsoft.Research.Liquid      // Get necessary Liquid libraries
open Util                           // General utilites
open Operations                     // Basic gates and operations
open Tests                          // Just gets us the RenderTest call for dumping files

module Script =                     // The script module allows for incremental loading



// Example functions
///////////////////////////////////////////

    let myCNOT (qs:Qubits) =
        let gate =
            Gate.Build("CNOT",fun () ->
                new Gate(
                    Name = "CNOT",
                    Help = "Controlled NOT",
                    Mat = (CSMat(4,[(0,0,1.,0.); (1,1,1.,0.);
                                    (2,3,1.,0.); (3,2,1.,0.)])),
                    Draw = "\\ctrl{#1}\\go[#1]\\targ"
                ))
        gate.Run qs
    
    let qcirc(qs:Qubits) = 
        M qs
    let sampleFunc (n:int) =
        let k = Ket(1)
        let stats = Array.create 2 0
        for i in 1 .. n do
            let qs = k.Reset(1)
            qcirc qs
            let v = qs.Head.Bit.v
            stats.[v] <- stats.[v] + 1
        show "Measurement statistics: 0=%d; 1=%d" stats.[0] stats.[1]

    let simpleCircuit (qs:Qubits) =
        H [qs.[0]]
        H [qs.[1]]
        myCNOT qs
        H [qs.[0]]
///////////////////////////////////////////


// Implement your functions here
///////////////////////////////////////////

///////////////////////////////////////////


    // program entry point
    [<LQD>]
    let Lab()    =
        show "Lab "
        (*sampleFunc 10000*)          // Test sampleFunc
         
        let k = Ket(2)
        let qs = k.Qubits
        show "State before: %s" (k.ToString())
        let compCirc = Circuit.Compile simpleCircuit qs
        compCirc.Render("SimpleCircuit.png")
        compCirc.GrowGates(k)
        compCirc.Run qs
        show "State after: %s" (k.ToString())   
        
        

        

#if INTERACTIVE
do Script.Lab()        // If interactive, then run the routine automatically
#endif
