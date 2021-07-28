extends Sprite

export(Color) var spriteColor = Color(1,1,1)
export(int) var layersize = 1
export(float) var twistAngle = 0
export(float) var rotationSpeed = 0
export(bool) var istargeting = false
export(Vector2) var target = Vector2()
export(bool) var layeredMode = false
export(Array) var layerRepetition = [1]

func spriteRotate(angle):
	for sprite in get_children():
		sprite.rotation += angle

func spriteRotateTo(angle):
	for sprite in get_children():
		sprite.rotation = angle

func spriteLookAt(vector):
	for sprite in get_children():
		sprite.look_at(vector)	

func _process(delta):
	self.spriteRotate(rotationSpeed * delta)
	if (self.istargeting): self.spriteLookAt(target)
	
func clear_sprites():
	for sprite in get_children():
		sprite.queue_free()

func _ready():
	self.modulate = spriteColor
	self.region_enabled = true

	render_sprites()
	self.spriteRotate(self.get_parent().rotation)

func render_sprites():
	clear_sprites()
	var currentHeight = 0
	for i in range(0, hframes):
		if (layeredMode):
			for _j in range(0,layerRepetition[i]):
				var next_sprite = Sprite.new()
				next_sprite.texture = texture
				next_sprite.hframes = hframes
				next_sprite.visible = true
				next_sprite.frame = i
				next_sprite.position.y = -layersize * (currentHeight)
				next_sprite.rotation = twistAngle * currentHeight
				currentHeight += 1
				add_child(next_sprite)
		else:
			var next_sprite = Sprite.new()
			next_sprite.texture = texture
			next_sprite.hframes = hframes
			next_sprite.visible = true
			next_sprite.frame = i
			next_sprite.position.y = -layersize * i
			add_child(next_sprite)
