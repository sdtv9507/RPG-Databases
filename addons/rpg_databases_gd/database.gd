tool
extends EditorPlugin

var instanced_scene: Control = null
var button: Button = Button.new()

func _enter_tree():
	instanced_scene = load("res://addons/rpg_databases_gd/Scenes/Base.tscn").instance()
	button.text = "Database"
	button.connect("pressed", self, "_on_button_pressed")
	add_control_to_container(EditorPlugin.CONTAINER_TOOLBAR, button)
	add_child(instanced_scene)


func _exit_tree():
	remove_child(instanced_scene)
	remove_control_from_container(EditorPlugin.CONTAINER_TOOLBAR, button)
	button.queue_free()
	instanced_scene.queue_free()

func _on_button_pressed():
	if (instanced_scene.visible == true):
		var pos: Vector2 = Vector2(150, 150)
		instanced_scene.set_position(pos)
		instanced_scene.show()
	else:
		instanced_scene.hide()
