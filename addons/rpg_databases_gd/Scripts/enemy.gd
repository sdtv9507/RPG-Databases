extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var graphics_path = ""
var enemy_selected = 0
var statEdit = -1
var drop_id_array: Array
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start():
	var json_dictionary = get_parent().get_parent().call("read_data", "Enemy")
	$EnemyButton.clear()
	for i in range(json_dictionary.size()):
		var enemy_data = json_dictionary["enemy" + String(i)]
		if (i > $EnemyButton.get_item_count() - 1):
			$EnemyButton.add_item(enemy_data["name"])
		else:
			$EnemyButton.set_item_text(i, enemy_data["name"])
	refresh_data(enemy_selected)

func refresh_data(id):
	var json_dictionary = get_parent().get_parent().call("read_data", "Enemy")
	var enemy_data = json_dictionary["enemy" + String(id)]
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_stats_data = json_dictionary["stats"]
	var item_list = get_parent().get_parent().call("read_data", "Item")
	var weapon_list = get_parent().get_parent().call("read_data", "Weapon")
	var armor_list = get_parent().get_parent().call("read_data", "Armor")

	$NameLabel/NameLine.text = enemy_data["name"]
	var graphic = enemy_data["graphicImage"]
	if (graphic != ""):
		graphics_path = enemy_data["graphicImage"]
		$GraphicLabel/Graphic.texture = load(enemy_data["graphicImage"])
	$StatsLabel/StatsContainer/DataContainer/StatList.clear()
	$StatsLabel/StatsContainer/DataContainer/FormulaList.clear()
	for i in range(system_stats_data.size()):
		var stat_name = system_stats_data[String(i)]
		var enemy_stat_formula = enemy_data["stat_list"]
		var stat_formula = "";
		if (enemy_stat_formula.has(stat_name)):
			stat_formula = enemy_stat_formula[stat_name]
		else:
			stat_formula = "level * 5"
		$StatsLabel/StatsContainer/DataContainer/StatList.add_item(stat_name)
		$StatsLabel/StatsContainer/DataContainer/FormulaList.add_item(stat_formula)
	var drop_list = enemy_data["drop_list"]
	$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.clear()
	$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList.clear()
	drop_id_array.clear()
	for drop in drop_list.keys():
		var kind = drop[0]
		var kind_id: String = drop
		kind_id.erase(0,1)
		match kind:
			"i":
				drop_id_array.append(drop)
				var item_data: Dictionary = item_list["item" + kind_id]
				$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(item_data["name"])
			"w":
				drop_id_array.append(drop)
				var weapon_data = weapon_list["weapon" + kind_id]
				$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(weapon_data["name"])
			"a":
				drop_id_array.append(drop)
				var armor_data = armor_list["armor" + kind_id]
				$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(armor_data["name"])
		$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList.add_item(String(drop_list[drop]))
	$ExpLabel/ExpSpin.value = int(enemy_data["experience"])
	$GoldLabel/GoldSpin.value = int(enemy_data["money"])
	
	if (enemy_data.has("effects") == true):
		clear_effect_list()
		var effect_list: Array = enemy_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), String(effect["value1"]), String(effect["value2"]))

func _on_AddEnemy_pressed():
	$EnemyButton.add_item("NewEnemy")
	var id = $EnemyButton.get_item_count() - 1;
	var json_dictionary = get_parent().get_parent().call("read_data", "Enemy")
	var enemy_data: Dictionary
	var stats_data: Dictionary
	var drop_data: Dictionary
	enemy_data["name"] = "Slime"
	enemy_data["graphicImage"] = ""
	stats_data["hp"] = "150"
	stats_data["mp"] = "50"
	stats_data["atk"] = "18"
	stats_data["def"] = "16"
	stats_data["int"] = "8"
	stats_data["res"] = "4"
	stats_data["spd"] = "12"
	stats_data["luk"] = "10"
	drop_data["i0"] = 80
	enemy_data["experience"] = 6
	enemy_data["money"] = 50
	enemy_data["stat_list"] = stats_data
	enemy_data["drop_list"] = drop_data
	json_dictionary["enemy" + String(id)] = enemy_data
	get_parent().get_parent().call("store_data", "Enemy", json_dictionary)

func _on_eraseEnemy_pressed():
	var json_dictionary = get_parent().get_parent().call("read_data", "Enemy")
	if (json_dictionary.keys().size() > 1):
		var enemyId = enemy_selected
		while (enemyId < json_dictionary.keys().size() - 1):
			json_dictionary["Enemy" + enemyId] = json_dictionary["Enemy" + (enemyId + 1)]
			enemyId += 1;
		json_dictionary.erase("Enemy" + enemyId)
		get_parent().get_parent().call("store_data", "Enemy", json_dictionary)
		$EnemyButton.remove_item(enemy_selected)
		if (enemy_selected == 0):
			$EnemyButton.select(enemy_selected + 1)
			enemy_selected += 1
		else:
			$EnemyButton.select(enemy_selected - 1)
			enemy_selected -= 1
		$EnemyButton.select(enemy_selected)
		refresh_data(enemy_selected)

func _on_SearchGraphic_pressed():
	$EnemyGraphic.popup_centered()

func _on_EnemyGraphic_file_selected(path):
	graphics_path = path;
	$GraphicLabel/Graphic.texture = load(path)

func _on_FormulaList_item_activated(var index):
	var stat_name = $StatsLabel/StatsContainer/DataContainer/StatList.get_item_text(index)
	var stat_formula = $StatsLabel/StatsContainer/DataContainer/FormulaList.get_item_text(index)
	$StatsEdit/Stat.text = stat_name
	$StatsEdit/Formula.text = stat_formula
	statEdit = index
	$StatsEdit.popup_centered()

func _on_EnemyStatEditorOkButton_pressed():
	var stat_formula = $StatsEdit/Formula.text;
	$StatsLabel/StatsContainer/DataContainer/FormulaList.set_item_text(statEdit, stat_formula)
	save_enemy_data()
	statEdit = -1
	$StatsEdit.hide()

func _on_EnemyStatEditorCancelButton_pressed():
	statEdit = -1;
	$StatsEdit.hide()

func _on_AddDrop_pressed():
	$DropEdit/Type/OptionButton.select(0)
	var item_data = get_parent().get_parent().call("read_data", "Item")
	for i in range(item_data.size()):
		var itemName = item_data["item" + String(i)]
		if (i > $DropEdit/Drop/OptionButton.get_item_count() - 1):
			$DropEdit/Drop/OptionButton.add_item(itemName["name"])
		else:
			$DropEdit/Drop/OptionButton.set_item_text(i, itemName["name"])
	$DropEdit.popup_centered()

func _on_eraseDrop_pressed():
	var id = $DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.get_selected_items()[0];
	$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.remove_item(id)
	$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList.remove_item(id)

func _on_DropType_item_selected(index):
	var item_data = get_parent().get_parent().call("read_data", "Item")
	var weapon_data = get_parent().get_parent().call("read_data", "Weapon")
	var armor_data = get_parent().get_parent().call("read_data", "Armor")
	$DropEdit/Drop/OptionButton.clear()
	match index:
		0:
			for i in range(item_data.size()):
				var itemName = item_data["item" + String(i)]
				if (i > $DropEdit/Drop/OptionButton.get_item_count() - 1):
					$DropEdit/Drop/OptionButton.add_item(itemName["name"])
				else:
					$DropEdit/Drop/OptionButton.set_item_text(i, itemName["name"])
		1:
			for i in range(weapon_data.size()):
				var weaponName = weapon_data["weapon" + String(i)]
				if (i > $DropEdit/Drop/OptionButton.get_item_count() - 1):
					$DropEdit/Drop/OptionButton.add_item(weaponName["name"])
				else:
					$DropEdit/Drop/OptionButton.set_item_text(i, weaponName["name"])
		2:
			for i in range(armor_data.size()):
				var armorName = armor_data["armor" + String(i)]
				if (i > $DropEdit/Drop/OptionButton.get_item_count() - 1):
					$DropEdit/Drop/OptionButton.add_item(armorName["name"])
				else:
					$DropEdit/Drop/OptionButton.set_item_text(i, armorName["name"])

func _on_DropEditOkButton_pressed():
	var item_list = get_parent().get_parent().call("read_data", "Item")
	var weapon_list = get_parent().get_parent().call("read_data", "Weapon")
	var armor_list = get_parent().get_parent().call("read_data", "Armor")
	var id = $DropEdit/Type/OptionButton.get_selected_id()
	var selected_id = $DropEdit/Drop/OptionButton.get_selected_id()
	var chance = int($DropEdit/Chance/SpinBox.value)
	match id:
		0:
			drop_id_array.append("i" + String(selected_id))
			var item_data = item_list["item" + String(selected_id)]
			$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(item_data["name"])
		1:
			drop_id_array.append("w" + String(selected_id))
			var weapon_data = weapon_list["weapon" + String(selected_id)]
			$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(weapon_data["name"])
		2:
			drop_id_array.append("a" + String(selected_id))
			var armor_data = armor_list["armor" + String(selected_id)]
			$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.add_item(armor_data["name"])
	$DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList.add_item(String(chance))
	$DropEdit.hide()

func _on_DropEditCancelButton_pressed():
	$DropEdit.hide()

func _on_EnemySaveButton_pressed():
	save_enemy_data()
	refresh_data(enemy_selected)

func save_enemy_data():
	var json_dictionary = get_parent().get_parent().call("read_data", "Enemy")
	var enemy_data = json_dictionary["enemy" + String(enemy_selected)]
	var stats_data = enemy_data["stat_list"]
	var drops_data = enemy_data["drop_list"]
	var effect_list: Array
	enemy_data["name"] = $NameLabel/NameLine.text;
	enemy_data["graphicImage"] = graphics_path;
	var items = $StatsLabel/StatsContainer/DataContainer/StatList.get_item_count()
	for i in range(items):
		var stat = $StatsLabel/StatsContainer/DataContainer/StatList.get_item_text(i)
		var formula = $StatsLabel/StatsContainer/DataContainer/FormulaList.get_item_text(i)
		stats_data[stat] = formula
	items = $DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList.get_item_count()
	for i in range(items):
		var id = drop_id_array[i]
		var chance = $DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList.get_item_text(i)
		drops_data[id] = chance
	var effect_size = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in range(effect_size):
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_item_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_item_text(i)
		effect_list.append(effect_data)
	enemy_data["effects"] = effect_list;
	enemy_data["experience"] = $ExpLabel/ExpSpin.value;
	enemy_data["money"] = $GoldLabel/GoldSpin.value;
	enemy_data["stat_list"] = stats_data;
	enemy_data["drop_list"] = drops_data;
	get_parent().get_parent().call("store_data", "Enemy", json_dictionary)

func _on_AddEnemyEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Enemy")

func _on_eraseEnemyEffect_pressed() -> void:
	var id = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(String(data_id))
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
