# ActionFlow
ActionFlow implements a redux/flux-like architecture for C#/.Net applications. 

Originally developed to streamline improvements to my AI assistant project, it allows application logic to be structured as a flow of actions occurring around a centralized state. 
ActionFlow allows for multiple action, effect, selector, and reducer, files. As long as a file implements the relevant interface, and is injected using that interface, it should be pulled into the flow automatically. 

See gptManager project for examples (https://github.com/bnesiba/gptManager). 
