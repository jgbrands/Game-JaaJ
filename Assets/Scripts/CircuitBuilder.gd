extends Path2D

export(float) var resolution = 20;
export(PackedScene) var circuit

func _ready():
	var dist = 1/resolution;
	var circuitNode = circuit.instance()
	self.add_child(circuitNode)
	for a in range(resolution + 1):
		get_node("Follow").set_unit_offset(dist * a)
		circuitNode.add_point(get_node("Follow").position)
	update()
