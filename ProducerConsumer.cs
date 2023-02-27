class Queue
{
    string[] buffer = new string[10];
    int front = -1, back = -1;
    int MAX_SIZE;

    public Queue(int MAX_SIZE)
    {
        this.MAX_SIZE = MAX_SIZE;
    }

    public bool QueueIsEmpty()
    {
        return front == -1 && back == -1;
    }

    public bool QueueIsFull()
    {
        return (front - back == MAX_SIZE) || front == MAX_SIZE;
    }

    public void Enqueue(string data)
    {
        if(QueueIsFull())
        {
            Console.WriteLine("Buffer is full!");
        }

        else 
        {
            if(QueueIsEmpty())
            {
                front = 0;
                back = 0;
            }

            buffer[front] = data;
            front += 1;

            Console.Write($"Enqueue successful {data}");
        }
    }

    public string Dequeue()
    {
        if(QueueIsEmpty())
        {
            Console.WriteLine("Buffer is Empty!");
            return "";
        }

        else 
        {
            string data = buffer[back];
            back++;

            if(back >= front)
            {
                back = -1;
                front = -1;
            }

            Console.Write($"Dequeue successful {data}");
            return data;
        }
    }

    public void PrintQueue()
    {
        Console.Write("Buffer ");
        for(int i=back; i<front; i++)
        {
            Console.Write($"{buffer[i]} ");
        }

        Console.Write("\n");
    }
}



class ProducerConsumer
{

    public bool mutex = false;
    public static int MAX_SIZE = 10;
    public Queue memory = new Queue(MAX_SIZE);

    private static Random random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void Produce()
    {
        while(true)
        {
            Thread.Sleep(random.Next(0, 5)*1000);
            memory.PrintQueue();

            if(mutex == false)
            {
                mutex = true;
                string data = RandomString(10);

                if(memory.QueueIsFull())
                {
                    Console.WriteLine("Buffer is full!");
                }
                
                else 
                {
                    Console.WriteLine($"Writing {data} to buffer ...");
                    Thread.Sleep(1000);
                    memory.Enqueue(data);
                    Console.WriteLine($"Data added to buffer {data}");
                }

                mutex = false;
            }

            else
            {
                Console.WriteLine("Buffer is busy!");
            }
        }
    }

    public void Consume()
    {
        while(true)
        {

            Thread.Sleep(random.Next(0, 5)*1000);
            memory.PrintQueue();

            if(mutex == false)
            {
                mutex = true;

                if(memory.QueueIsEmpty())
                {
                    Console.WriteLine("Buffer is Empty!");
                }

                else
                {
                    Console.WriteLine($"Consuming data from the buffer ...");
                    Thread.Sleep(1000);
                    string data = memory.Dequeue();
                    Console.WriteLine($"{data} removed from buffer");
                }

                mutex = false;

            }

            else
            {
                Console.WriteLine("Buffer busy");
            }
        }

    }


    public static void Main(String[] args)
    {
        ProducerConsumer pc = new ProducerConsumer();

        ThreadStart threadConsumer = new ThreadStart(pc.Consume);
        ThreadStart threadProducer = new ThreadStart(pc.Produce);

        Thread Consumer = new Thread(threadConsumer);
        Thread Producer = new Thread(threadProducer);

        Consumer.Start();
        Producer.Start();

    }
}