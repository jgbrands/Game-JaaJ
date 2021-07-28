tool
extends Path2D

export(PackedScene) var object
export(float) var numberOfObjects = 3

func _ready():
	var dist = 1/numberOfObjects
	for a in range(numberOfObjects):
		get_node("Follow").set_unit_offset(dist * a)
		var next_object = object.instance()
		next_object.position = get_node("Follow").position
		self.get_node("YSort").add_child(next_object)
	update()
