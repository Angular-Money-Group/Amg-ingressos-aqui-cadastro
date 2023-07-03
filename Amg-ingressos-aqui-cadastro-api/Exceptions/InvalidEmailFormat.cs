
        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}