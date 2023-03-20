using System;
using System.Runtime.InteropServices;

class Program
{
    // Import the Rust library
    [DllImport("lib/ichor.dll")]
    public static extern IntPtr node_new(float x, float y);

    [DllImport("lib/ichor.dll")]
    public static extern void node_free(IntPtr ptr);

    [DllImport("lib/ichor.dll")]
    public static extern IntPtr node_string(Node node);


    // Define the Node struct in C#
    [StructLayout(LayoutKind.Sequential)]
    public struct Node
    {
        public float x;
        public float y;
    }

    static void Main(string[] args)
    {
        // Create a new Node in Rust and get a pointer to it
        IntPtr nodePtr = node_new(6.0f, 2.0f);
        if (nodePtr == IntPtr.Zero)
        {
            Console.WriteLine("Failed to create node!");
            return;
        }

        // Get the Node struct from the pointer
        Node node = Marshal.PtrToStructure<Node>(nodePtr);

        // Use the node_add function to get a string representation of the Node
        IntPtr strPtr = node_string(node);
        if (strPtr == IntPtr.Zero)
        {
            Console.WriteLine("Failed to create string!");
            return;
        }

        // Convert the C string to a C# string
        string nodeStr = Marshal.PtrToStringAnsi(strPtr);

        // Free the Node and string memory
        node_free(nodePtr);
        Marshal.FreeHGlobal(strPtr);

        // Print the Node string
        Console.WriteLine(nodeStr);
    }
}
