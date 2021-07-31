extends Control

onready var tutorial = self.get_node("CanvasLayer/Popup")

func _ready():
	pass


func _on_StartButton_pressed():
	get_tree().change_scene("res://Scenes/CityLevels/City-001.tscn");


func _on_Options_pressed():
	pass # Replace with function body.


func _on_Tutorial_pressed():
	tutorial.popup() # Replace with function body.
