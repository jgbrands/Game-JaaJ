extends Sprite

export(Color) var spriteColor = Color(1,1,1)

func _process(delta):
	for sprite in get_children():
		sprite.rotation += delta
	self.region_enabled = true

func clear_sprites():
	for sprite in get_children():
		sprite.queue_free()

func _ready():
	self.modulate = spriteColor
	render_sprites()

func render_sprites():
	clear_sprites()
	for i in range(0, hframes):
		var next_sprite = Sprite.new()
		next_sprite.texture = texture
		next_sprite.hframes = hframes
		next_sprite.visible = true;
		next_sprite.frame = i
		next_sprite.position.y = -i
		add_child(next_sprite)
